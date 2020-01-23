using ReactiveUI;
using ReactiveUI.XamForms;
using System.Reactive;
using System.Reactive.Linq;
using Xamarin.Forms;

namespace Forms
{
    public partial class MainPage : ReactiveContentPage<MainPageViewModel>
    {
        public MainPage()
        {
            InitializeComponent();

            //RefreshView
            //    .Events()
            //    .Refreshing
            //    .Select(_ => Unit.Default)
            //    .InvokeCommand(this, x => x.ViewModel.RefreshCommand);

            this.Bind(ViewModel, x => x.IsRefreshing, x => x.RefreshView.IsRefreshing);
            this.OneWayBind(ViewModel, x => x.IsRefreshing, x => x.Indicator.IsRunning);
            this.BindCommand(ViewModel, x => x.RxRefreshCommand, x => x.RefreshView);
            this.BindCommand(ViewModel, x => x.RxRefreshCommand, x => x.RefreshButton);
        }
    }
}
