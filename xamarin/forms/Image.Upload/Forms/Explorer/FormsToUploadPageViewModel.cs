using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Forms.Data;
using Forms.Logging;
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
        private readonly IFormsService _formsService;
        private readonly ILogs _logs;
        private ObservableAsPropertyHelper<int> _numberOfItemsQueued;
        public IEnumerable<UploadPayload> UploadPayloads { get; set; }
        public string Id => "FormsToUpload";
        public FormsToUploadPageViewModel(IUploadService uploadService = null,
                                          IFormsService formsService = null,
                                          ILogs logs = null)
        {
            _uploadService = uploadService ?? Locator.Current.GetService<IUploadService>();
            _formsService = formsService ?? Locator.Current.GetService<IFormsService>();
            _logs = logs ?? Locator.Current.GetService<ILogs>();

            UploadPayloads = new List<UploadPayload>();

            Upload = ReactiveCommand.Create(() => _uploadService.Queue(new UploadPayload()));

            LoadCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var payloads = await _formsService.GetPayloads();
                return payloads;
            });

            LoadCommand
                .ThrownExceptions
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x =>
                {
                    _logs.Log(x);
                });

            //_numberOfItemsQueued = this.WhenAnyObservable(x => _imageUploadService.Queued)
            //    .Where(x => x.State == UploadState.Queued)
            //    .Aggregate(0, (i, args) => i++)
            //    .ToProperty(this, x => x.QueuedItems);
        }

        public int QueuedItems => _numberOfItemsQueued.Value;
        public ReactiveCommand<Unit, Unit> Upload { get; set; }        
        public ReactiveCommand<Unit, Unit> QueueUpload { get; set; }
        public ReactiveCommand<Unit, IEnumerable<UploadPayload>> LoadCommand { get; set; }
    }
}