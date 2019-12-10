using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Punchclock;

namespace Forms.Services
{
    public class ImageUploadService : IImageUploadService
    {
        private readonly Subject<UploadEventArgs> _queueSubject = new Subject<UploadEventArgs>();
        private readonly OperationQueue _opQueue = new OperationQueue(2 /*at a time*/);

        public void Queue(MyTestPayload type)
        {
            _queueSubject.OnNext(new UploadEventArgs { Id = type.Id, State = UploadState.Queued });
            var dequeueObservable = _opQueue.Enqueue(1, async () => await UploadImage(type)).ToObservable();

            dequeueObservable
                .Subscribe(_ => _queueSubject.OnNext(new UploadEventArgs {Id = type.Id, State = UploadState.Dequeued}));
        }

        public IObservable<bool> ToggleService() => Observable.Return(true);

        public IObservable<UploadEventArgs> Queued => _queueSubject.AsObservable();

        private async Task UploadImage(MyTestPayload payload)
        {
            _queueSubject.OnNext(new UploadEventArgs{ Id = payload.Id, State = UploadState.UploadStarted });
            
            // Send your network call
            await Task.CompletedTask;
            
            _queueSubject.OnNext(new UploadEventArgs{ Id = payload.Id, State = UploadState.UploadCompleted });
        }
    }
}