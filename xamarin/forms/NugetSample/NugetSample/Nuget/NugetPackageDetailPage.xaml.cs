using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using ReactiveUI;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NugetSample.Nuget
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NugetPackageDetailPage
    {
        protected readonly CompositeDisposable ViewBindings = new CompositeDisposable();

        public NugetPackageDetailPage()
        {
            InitializeComponent();

            this.WhenAnyValue(x => x.ViewModel.PackageSearchMetadata)
                .Where(x => x != null)
                .InvokeCommand(this, x => x.ViewModel.GetVersions)
                .DisposeWith(ViewBindings);

            NuGetVersions
                .Events()
                .ItemSelected
                .Subscribe(_ => NuGetVersions.SelectedItem = null)
                .DisposeWith(ViewBindings);
        }
    }
}