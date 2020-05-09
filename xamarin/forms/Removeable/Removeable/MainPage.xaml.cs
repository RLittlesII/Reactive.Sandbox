using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.XamForms;
using Xamarin.Forms;

namespace Removeable
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ReactiveContentPage<MainViewModel>
    {
        private readonly CompositeDisposable ViewBindings = new CompositeDisposable();

        public MainPage()
        {
            InitializeComponent();

            this.WhenAnyValue(x => x.ViewModel.Items)
                .Where(x => x != null)
                .BindTo(this, x => x.List.ItemsSource)
                .DisposeWith(ViewBindings);

            ViewModel = new MainViewModel(new CoffeeDataService(new CoffeeClientMock()));
        }
    }
}
