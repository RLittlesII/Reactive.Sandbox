using ReactiveUI;
using ReactiveUI.XamForms;
using Xamarin.Forms;

namespace Forms
{
    public partial class MainPage : ReactiveContentPage<MainPageViewModel>
    {
        public MainPage()
        {
            InitializeComponent();

            RefreshView
                .Events()
                .Refreshing
                .InvokeCommand(this, x => x.ViewModel.RefreshCommand);

            this.OneWayBind(ViewModel, x => x.IsRefreshing, x => x.RefreshView.IsRefreshing);
        }
    }
}
