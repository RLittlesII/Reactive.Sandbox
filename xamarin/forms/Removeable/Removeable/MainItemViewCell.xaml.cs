using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.XamForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Removeable
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainItemViewCell : ReactiveViewCell<MainItemViewModel>
    {
        public MainItemViewCell()
        {
            InitializeComponent();

            this.OneWayBind(ViewModel, x => x.Title, x => x.Title.Text);

            this.BindCommand(ViewModel, x => x.Remove, x => x.Remove);
        }
    }
}