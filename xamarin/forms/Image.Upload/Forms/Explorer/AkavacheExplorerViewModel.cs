﻿using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Forms.Services;
using ReactiveUI;
using Sextant;

namespace Forms.Explorer
{
    public class AkavacheExplorerViewModel : ReactiveObject, IViewModel
    {
        private readonly IImageUploadService _imageUploadService;
        private ObservableAsPropertyHelper<int> _numberOfItemsQueued;

        public string Id => "Akavache Explorer";

        public AkavacheExplorerViewModel(IImageUploadService imageUploadService)
        {
            _imageUploadService = imageUploadService;

            Upload = ReactiveCommand.Create(() => _imageUploadService.Queue(new MyTestPayload()));

            _numberOfItemsQueued = this.WhenAnyObservable(x => _imageUploadService.Queued)
                .Where(x => x.State == UploadState.Queued)
                .Aggregate(0, (i, args) => i++)
                .ToProperty(this, x => x.QueuedItems);
        }

        public int QueuedItems => _numberOfItemsQueued.Value;

        public ReactiveCommand<Unit, Unit> Upload { get; set; }
        
        public ReactiveCommand<Unit, Unit> QueueUpload { get; set; }
    }
}