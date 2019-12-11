using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class AkavacheExplorer : ReactiveContentPage<AkavacheExplorerViewModel>
    {
        public AkavacheExplorer()
        {
            InitializeComponent();

            this.OneWayBind(ViewModel, x => x.QueuedItems, x => x.QueuedCount.Text);

            Queue
                .Events()
                .Clicked
                .ObserveOn(RxApp.MainThreadScheduler)
                .InvokeCommand(this, x => x.ViewModel.QueueUpload);

            this.WhenAnyObservable(x => x.ViewModel.QueueUpload.IsExecuting).Subscribe(_ => { });
        }
    }
}