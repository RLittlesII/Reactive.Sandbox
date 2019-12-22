using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Forms.Types;
using Punchclock;
using ReactiveUI;

namespace Forms.Services
{
    public class UploadService : IUploadService, IDisposable
    {
        private readonly Subject<UploadEventArgs> _queueSubject = new Subject<UploadEventArgs>();
        private readonly OperationQueue _opQueue = new OperationQueue(2 /*at a time*/);

        public UploadService()
        {

        }
        public void Queue(UploadPayload payload)
        {
            try
            {
                _queueSubject.OnNext(new UploadEventArgs { Id = payload.Form.Id, State = UploadState.Queued });
                var dequeueObservable = _opQueue.Enqueue(1, async () => await UploadImage(payload)).ToObservable();

                dequeueObservable
                    .Subscribe(_ => _queueSubject.OnNext(new UploadEventArgs { Id = payload.Form.Id, State = UploadState.Dequeued }));
            }
            catch (Exception exception)
            {
                _queueSubject.OnNext(new UploadEventArgs { Id = payload.Form.Id, State = UploadState.Errored });
                throw;
            }
        }

        public void Queue(IEnumerable<UploadPayload> payloads)
        {

        }

        public IObservable<Unit> Resume() =>
            Observable.Return(Unit.Default).Do(_ => _opQueue.ShutdownQueue()); // TODO: Figure out how to pause and resume the queue

        public IObservable<Unit> Pause() => Observable.Return(Unit.Default).Do(_ => _opQueue.PauseQueue());

        public IObservable<UploadEventArgs> Queued => _queueSubject.AsObservable();

        public UploadPayload CurrentUpload { get; }

        private async Task UploadImage(UploadPayload payload)
        {
            try
            {
                _queueSubject.OnNext(new UploadEventArgs { Id = payload.Form.Id, State = UploadState.UploadStarted });

                // Send your network call
                await Task.CompletedTask;

                _queueSubject.OnNext(new UploadEventArgs { Id = payload.Form.Id, State = UploadState.UploadCompleted });
            }
            catch (Exception e)
            {
                _queueSubject.OnNext(new UploadEventArgs { Id = payload.Form.Id, State = UploadState.Errored });
                throw;
            }
        }

        public void Dispose()
        {
            _queueSubject.OnCompleted();
            _queueSubject?.Dispose();
            _opQueue?.Dispose();
        }
    }
}