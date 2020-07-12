using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NugetSample.Nuget
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NuGetViewCell
    {
        protected readonly CompositeDisposable ViewCellBindings = new CompositeDisposable();

        public NuGetViewCell()
        {
            InitializeComponent();

            this.OneWayBind(ViewModel, x => x.PackageMetadata.Identity.Id, x => x.PackageName.Text)
                .DisposeWith(ViewCellBindings);

            this.OneWayBind(ViewModel, x => x.PackageMetadata.Authors, x => x.Authors.Text, text => $"by: {text}")
                .DisposeWith(ViewCellBindings);
        
            this.OneWayBind(ViewModel, x => x.PackageMetadata.IconUrl, x => x.IconUrl.Uri)
                .DisposeWith(ViewCellBindings);

            // Note: The VersionToStringConvterer could be done inline.  Check the ReadMe.md for a link to the documentation
            this.OneWayBind(ViewModel, x => x.PackageMetadata.Identity.Version, x => x.PackageVersion.Text, vmToViewConverterOverride: new VersionToStringConverter())
                .DisposeWith(ViewCellBindings);

            this.OneWayBind(ViewModel, x => x.PackageMetadata.DownloadCount, x => x.DownloadCount.Text, count => $"{count:N0} downloads")
                .DisposeWith(ViewCellBindings);
        }
    }
}