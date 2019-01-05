using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.XamForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GH1873
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SecondView : ReactiveContentPage<SecondViewModel>
    {
		public SecondView()
		{
			InitializeComponent ();

		    this.BindCommand(ViewModel, vm => vm.NavigateCommand, view => view.Navigate);

		    this.BindCommand(ViewModel, vm => vm.NavigateBackCommand, view => view.NavigateBack);
        }
	}
}