using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.XamForms;
using Xamarin.Forms;

namespace ReactiveUI2466
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ReactiveContentPage<MainViewModel>
    {
        public MainPage()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.IsReadOnly, x => x.ExportName.IsEnabled,
                    arg =>
                    {
                        if (arg)
                            return false;
                        else return true;
                    });
        }
    }
}
