using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.XamForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Main
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : ReactiveContentPage<MainViewModel>
    {
        public MainPage()
        {
			InitializeComponent();

            this.WhenActivated(d =>
            {
                d(this.OneWayBind(ViewModel, vm => vm.MenuItems, v => v.items.ItemsSource));

                d(Observable
                    .FromEventPattern(items, nameof(items.ItemTapped))
                    .Where(x => x.EventArgs is ItemTappedEventArgs)
                    .Select(x => (x.EventArgs as ItemTappedEventArgs).Item)
                    .InvokeCommand(ViewModel.NavigateToMenuItem));
            });
        }
    }
}
