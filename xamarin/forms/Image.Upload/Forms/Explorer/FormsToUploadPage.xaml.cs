using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.XamForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Forms.Explorer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FormsToUploadPage : ContentPageBase<FormsToUploadPageViewModel>
    {
        public FormsToUploadPage()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                this
                .WhenAnyValue(v => v.ViewModel.LoadCommand)
                .Where(x => !Loaded)
                .Do(x => Loaded = true)
                .Select(x => Unit.Default)
                .InvokeCommand(ViewModel.LoadCommand)
                .DisposeWith(disposables);

                this.OneWayBind(ViewModel,
                                x => x.QueuedItems,
                                x => x.QueuedCount.Text);

                this
                    .OneWayBind(ViewModel,
                                x => x.UploadPayloads,
                                x => x.UploadsList.ItemsSource);
                AddNewPayload
                    .Events()
                    .Clicked
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .InvokeCommand(this, x => x.ViewModel.AddUploadPayloadCommand);

                Queue
                    .Events()
                    .Clicked
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .InvokeCommand(this, x => x.ViewModel.QueueUpload);

                this.WhenAnyObservable(x => x.ViewModel.QueueUpload.IsExecuting).Subscribe(_ => { });
            });
        }
    }
}