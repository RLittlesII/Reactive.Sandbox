using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Forms.Types;
using Punchclock;

namespace Forms.Services
{
    public class UploadService : IUploadService, IDisposable
    {
        private readonly Subject<UploadEventArgs> _queueSubject = new Subject<UploadEventArgs>();
        private readonly OperationQueue _opQueue = new OperationQueue(2 /*at a time*/);

        public void Queue(UploadPayload payload)
        {
            _queueSubject.OnNext(new UploadEventArgs { Id = payload.Form.Id, State = UploadState.Queued });
            var dequeueObservable = _opQueue.Enqueue(1, async () => await UploadImage(payload)).ToObservable();

            var disposable = dequeueObservable
                .Subscribe(_ => _queueSubject.OnNext(new UploadEventArgs { Id = payload.Form.Id, State = UploadState.Dequeued }));
        }

        public void Queue(IEnumerable<UploadPayload> payloads)
        {

        }

        public IObservable<bool> ToggleService() => Observable.Return(true);

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