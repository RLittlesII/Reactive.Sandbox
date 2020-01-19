using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace Forms.Services
{
    public class QueueService
    {
        Subject<IObservable<DateTimeOffset>> _worker = new Subject<IObservable<DateTimeOffset>>();
        List<IObservable<DateTimeOffset>> _buffer = new List<IObservable<DateTimeOffset>>();
        BehaviorSubject<bool> _pause = new BehaviorSubject<bool>(false);

        public QueueService(int maxConcurrentProcesses = 3)
        {
                _worker
                    .Publish()
                    .RefCount()
                    .CombineLatest(_pause, (observable, pause) => (observable, pause))
                    .SelectMany(Process)
                    .Merge(maxConcurrentProcesses)
                    .Subscribe();
        }

        private IObservable<IObservable<DateTimeOffset>> Process((IObservable<DateTimeOffset> observable, bool pause) _)
        {
            var (observable, pause ) = _;
            if (pause)
            {
                _buffer.Add(observable);
                return Observable.Empty<IObservable<DateTimeOffset>>();
            }

            if (_buffer.Any())
            {
                var result =
                    Observable
                        .Concat(_buffer.ToArray().ToObservable(), Observable.Return(observable));
                _buffer.Clear();
                return result;
            }

            return Observable.Return(observable);
        }

        public IObservable<T> Enqueue<T>(IObservable<T> request)
        {
            var observable = Observable.Create<T>(observer =>
            {
                _worker
                    .OnNext(request
                        .Do(observer)
                        .Materialize()
                        .Where(notification => notification.Kind == NotificationKind.OnCompleted)
                        .Select(x => DateTimeOffset.Now));

                return Disposable.Empty;
            });

            return observable.Publish().RefCount();
        }

        public Task<T> EnqueueTask<T>(Func<Task<T>> request) =>
            Enqueue(Observable.FromAsync(request)).ToTask();

        public Task<T> EnqueueTask<T>(Func<CancellationToken, Task<T>> request) =>
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
    }
}