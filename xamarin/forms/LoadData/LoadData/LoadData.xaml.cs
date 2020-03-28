using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using ReactiveUI;
using ReactiveUI.XamForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LoadData
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadData : ReactiveContentPage<LoadDataViewModel>
    {
        public LoadData()
        {
            InitializeComponent();

            this.WhenAnyValue(x => x.ViewModel.InitializeData)
                .Where(x => x != null)
                .SelectMany(cmd => cmd.Execute())
                .Subscribe();

            this.WhenAnyValue(x => x.ViewModel.InitializeData)
                .Where(x => x != null)
                .Select(x => Unit.Default)
                .InvokeCommand(this, x => x.ViewModel.InitializeData);

            ViewModel = new LoadDataViewModel();
        }
    }
}