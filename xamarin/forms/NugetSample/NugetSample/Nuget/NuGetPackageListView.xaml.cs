using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using ReactiveUI;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NugetSample.Nuget
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NuGetPackageListView
    {
        protected readonly CompositeDisposable ViewBindings = new CompositeDisposable();

        public NuGetPackageListView()
        {
            InitializeComponent();
            

            this.Bind(ViewModel, x => x.SearchText, x => x.SearchBar.Text)
                .DisposeWith(ViewBindings);

            this.OneWayBind(ViewModel, x => x.Instructions, x => x.Instructions.Text)
                .DisposeWith(ViewBindings);

            this.OneWayBind(ViewModel, x => x.HasItems, x => x.Instructions.IsVisible, vmToViewConverterOverride: new InverseBooleanBindingTypeConverter())
                .DisposeWith(ViewBindings);

            this.OneWayBind(ViewModel, x => x.HasItems, x => x.ListView.IsVisible)
                .DisposeWith(ViewBindings);

            this.OneWayBind(ViewModel, x => x.IsRefreshing, x => x.ListView.IsRefreshing)
                .DisposeWith(ViewBindings);

            this.WhenAnyValue(x => x.ViewModel.SearchResults)
                .BindTo(this, x => x.ListView.ItemsSource)
                .DisposeWith(ViewBindings);

            ListView
                .Events() // The Events API provided by Pharmacist
                .ItemTapped
                .Select(x => x.Item as NuGetPackageViewModel) // Select the tapped item as the View Model
                .ObserveOn(RxApp.MainThreadScheduler)
                .InvokeCommand(this, x => x.ViewModel.PackageDetails) // Invoke a command on the provided ViewModel
                .DisposeWith(ViewBindings);

            ListView
                .Events() // The Events API provided by Pharmacist
                .ItemSelected
                .Subscribe(x => ListView.SelectedItem = null)
                .DisposeWith(ViewBindings);

            ListView
                .Events() // The Events API provided by Pharmacist
                .Refreshing
                .Select(_ => Unit.Default)
                .InvokeCommand(this, x => x.ViewModel.Refresh)
                .DisposeWith(ViewBindings);
        }
    }
}