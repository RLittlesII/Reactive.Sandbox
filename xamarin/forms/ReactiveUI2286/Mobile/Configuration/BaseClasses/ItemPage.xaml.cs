using System;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.XamForms;
using Shared.Configuration;
using Xamarin.Forms.Xaml;

namespace Mobile.Configuration
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ItemPage : ReactiveContentPage<ItemViewModel>
    {
        public ItemPage()
        {
			InitializeComponent();

            this.WhenActivated(d =>
            {
                d(this.OneWayBind(ViewModel, vm => vm.Id, v => v.id.Text));
                d(this.OneWayBind(ViewModel, vm => vm.Name, v => v.name.Text));
                d(this.BindCommand(ViewModel, vm => vm.Confirm, v => v.save));
                d(this.BindCommand(ViewModel, vm => vm.Delete, v => v.delete));
            });
        }
    }
}
