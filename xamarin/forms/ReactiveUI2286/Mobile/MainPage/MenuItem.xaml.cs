using ReactiveUI;
using ReactiveUI.XamForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Main
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuItem : ReactiveViewCell<MenuItemViewModel>
    {
        public MenuItem()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                d(this.OneWayBind(ViewModel, vm => vm.Title, v => v.title.Text));
                d(this.OneWayBind(ViewModel, vm => vm.Icon, v => v.icon.Text));
            });
        }
    }
}