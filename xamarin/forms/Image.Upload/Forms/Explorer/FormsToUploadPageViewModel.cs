using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Forms.Data;
using Forms.Logging;
using Forms.Services;
using Forms.Types;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
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
        [Reactive]public List<UploadPayload> UploadPayloads { get; set; }
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

            AddUploadPayloadCommand = ReactiveCommand.CreateFromTask<EventArgs, UploadPayload>(async x =>
            {
                var payload = await CreatePayload();
                await _formsService.InsertPayload(payload);
                return payload;
            });

            InvalidatePayloadsCommand = ReactiveCommand.CreateFromTask<EventArgs, Unit>(async x =>
            {
                await _formsService.InvalidateAllPayloads();
                return Unit.Default;
            });

            RefreshListCommand = ReactiveCommand.CreateFromTask<EventArgs, IEnumerable<UploadPayload>>(async x =>
            {
                var payloads = await _formsService.GetPayloads();
                return payloads;
            });

            RefreshListCommand
                .Select(x => x)
                .Subscribe(x =>
                {
                    UploadPayloads = x.ToList();
                });
            //_numberOfItemsQueued = this.WhenAnyObservable(x => _imageUploadService.Queued)
            //    .Where(x => x.State == UploadState.Queued)
            //    .Aggregate(0, (i, args) => i++)
            //    .ToProperty(this, x => x.QueuedItems);
        }
        private async Task<UploadPayload> CreatePayload()
        {
            RandomGenerator generator = new RandomGenerator();

            var form = new Form()
            {
                Id = generator.RandomId(3, 3),
                FormName = generator.RandomString(4, true)
            };

            var images = new List<Image>();

            for (var i = 0; i < generator.RandomNumber(1, 5); i++)
            {
                var image = new Image()
                {
                    Id = generator.RandomId(3, 3),
                    FileLocation = generator.RandomString(6, true),
                    FormId = form.Id
                };
                images.Add(image);
            }

            var payload = new UploadPayload()
            {
                Form = form,
                Id = generator.RandomId(3, 3),
                Images = images
            };
            return payload;
        }

        public int QueuedItems => _numberOfItemsQueued.Value;
        public ReactiveCommand<Unit, Unit> Upload { get; set; }        
        public ReactiveCommand<Unit, Unit> QueueUpload { get; set; }
        public ReactiveCommand<Unit, IEnumerable<UploadPayload>> LoadCommand { get; set; }
        public ReactiveCommand<EventArgs, UploadPayload> AddUploadPayloadCommand { get; set; }
        public ReactiveCommand<EventArgs, Unit> InvalidatePayloadsCommand { get; set; }
        public ReactiveCommand<EventArgs, IEnumerable<UploadPayload>> RefreshListCommand { get; set; }
    }
    public class RandomGenerator
    {
        // Generate Id
        public string RandomId(int num, int chars)
        {
            return $"{RandomNumber(0, 999)}{RandomString(3, true)}";
        }
        // Generate a random number between two numbers    
        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        // Generate a random string with a given size    
        public string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        // Generate a random password    
        public string RandomPassword()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(2, false));
            return builder.ToString();
        }
    }
}