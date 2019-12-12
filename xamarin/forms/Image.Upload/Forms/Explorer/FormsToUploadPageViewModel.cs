using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Forms.Services;
using Forms.Types;
using ReactiveUI;
using Sextant;
using Splat;

namespace Forms.Explorer
{
    public class FormsToUploadPageViewModel : ReactiveObject, IViewModel
    {
        private readonly IUploadService _uploadService;
        private ObservableAsPropertyHelper<int> _numberOfItemsQueued;

        public string Id => "FormsToUpload";

        public FormsToUploadPageViewModel(IUploadService uploadService = null)
        {
            _uploadService = uploadService ?? Locator.Current.GetService<IUploadService>();

            Upload = ReactiveCommand.Create(() => _uploadService.Queue(new UploadPayload()));

            //_numberOfItemsQueued = this.WhenAnyObservable(x => _imageUploadService.Queued)
            //    .Where(x => x.State == UploadState.Queued)
            //    .Aggregate(0, (i, args) => i++)
            //    .ToProperty(this, x => x.QueuedItems);
        }

        public int QueuedItems => _numberOfItemsQueued.Value;
        public ReactiveCommand<Unit, Unit> Upload { get; set; }        
        public ReactiveCommand<Unit, Unit> QueueUpload { get; set; }
    }
}