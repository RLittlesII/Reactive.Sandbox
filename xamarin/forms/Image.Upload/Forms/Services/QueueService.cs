using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace Forms.Services
{
    public class QueueService : IDisposable
    {
        readonly Subject<IObservable<Unit>> _worker = new Subject<IObservable<Unit>>();
        readonly List<IObservable<Unit>> _buffer = new List<IObservable<Unit>>();
        readonly BehaviorSubject<bool> _pause = new BehaviorSubject<bool>(false);
        readonly CompositeDisposable _disposables = new CompositeDisposable();

        public QueueService(int maxConcurrentProcesses = 3)
        {
            _worker
                .Publish()
                .RefCount()
                .CombineLatest(_pause, (observable, pause) => (observable, pause)) // combine the latest notification of the operation and the worker state.
                .Where(x => x.observable != null)
                .SelectMany(Process) // Expand the operations
                .Merge(maxConcurrentProcesses) // Merge the operations, and only allow three to be processed at a time
                .Subscribe()
                .DisposeWith(_disposables);
        }

        public IObservable<T> Enqueue<T>(IObservable<T> request)
        {
            // Create an observable that tracks the completion of a request.
            var observable =
                Observable
                    .Create<T>(observer =>
                    {
                        // When the request completes tick the worker to pick up the next request
                        _worker
                            .OnNext(
                                request
                                    .Do(observer) // Observer performs OnNext, OnComplete, OnError; forces our function call
                                    .Materialize() // Get the observable of notifications
                                    .Where(notification => notification.Kind == NotificationKind.OnCompleted) // Get the completion notification
                                    .Select(x => Unit.Default));

                        return Disposable.Empty;
                    });

            return observable.Publish().RefCount();  // Return an observable that will only fire subscription once.
        }

        public Task<T> Enqueue<T>(Func<Task<T>> request) =>
            Enqueue(Observable.FromAsync(request)).ToTask();

        public Task<T> Enqueue<T>(Func<CancellationToken, Task<T>> request) =>
            Enqueue(Observable.FromAsync(request)).ToTask();

        public IObservable<bool> Pause()
        {
            _pause.OnNext(true);
            return Observable.Return(true);
        }

        public IObservable<bool> Resume()
        {
            _pause.OnNext(false);
            return Observable.Return(false);
        }

        private IObservable<IObservable<Unit>> Process((IObservable<Unit> observable, bool pause) _)
        {
            var (observable, pause) = _;
            if (pause)
            {
                // If the queue is paused add the items to the buffer and return.
                _buffer.Add(observable);
                return Observable.Empty<IObservable<Unit>>();
            }

            if (_buffer.Any())
            {
                // Concatenate the new observable values to the end of the buffer.
                var result =
                    Observable
                        .Concat(_buffer.ToArray().ToObservable(), Observable.Return(observable));
                _buffer.Clear(); // Clear the buffer
                return result; // return the concatenated result.
            }

            return Observable.Return(observable);
        }

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _disposables.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}