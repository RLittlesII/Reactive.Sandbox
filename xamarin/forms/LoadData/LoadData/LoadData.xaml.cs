using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using ReactiveUI;
using ReactiveUI.XamForms;
using Services.Coffee;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LoadData
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadData : ContentPageBase<LoadDataViewModel>
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

            this.WhenAnyValue(x => x.ViewModel.Items)
                .Where(x => x != null)
                .BindTo(this, x => x.LoadedList.ItemsSource)
                .DisposeWith(ViewBindings);

            ViewModel = new LoadDataViewModel(new CoffeeBeanDataService(new CoffeeBeanClientMock()));
        }
    }
}