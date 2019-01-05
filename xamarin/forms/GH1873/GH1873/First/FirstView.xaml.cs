using ReactiveUI;
using ReactiveUI.XamForms;
using Xamarin.Forms.Xaml;

namespace GH1873
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FirstView : ReactiveContentPage<FirstViewModel>
    {
        public FirstView()
        {
            InitializeComponent();

            this.BindCommand(ViewModel, vm => vm.NavigateCommand, view => view.Navigate);
        }
    }
}