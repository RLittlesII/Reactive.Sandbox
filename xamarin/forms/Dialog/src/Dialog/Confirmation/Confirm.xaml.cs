using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Windows.Input;
using ReactiveUI;
using Rg.Plugins.Popup.Pages;
using Rocket.Surgery.Airframe.Popup;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dialog
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfirmPopup : PopupPageBase<ConfirmViewModel>
    {
        public ConfirmPopup()
        {
            InitializeComponent();

            Confirm
                .Events()
                .Clicked
                .InvokeCommand(this, x => x.ViewModel.ConfirmCommand);
        }

        public bool Result { get; set; }
    }

    public interface IConfirmPage
    {
        bool Result { get; set; }
    }
}