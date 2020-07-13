using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NugetSample.Services;
using ReactiveUI;
using Sextant;

namespace NugetSample.Nuget
{
    public class NuGetPackageDetailViewModel : NavigableVieModel
    {
        private readonly INuGetPackageService _nuGetPackageService;
        private readonly ObservableAsPropertyHelper<IEnumerable<NuGetVersionViewModel>> _versions;
        private IPackageSearchMetadata _packageSearchMetadata;

        public NuGetPackageDetailViewModel(INuGetPackageService nuGetPackageService)
        {
            _nuGetPackageService = nuGetPackageService;
            GetVersions = ReactiveCommand.CreateFromTask<IPackageSearchMetadata, IEnumerable<NuGetVersionViewModel>>(ExecuteGetVersions);

            _versions = GetVersions.ToProperty(this, x => x.Versions, scheduler: RxApp.MainThreadScheduler);
        }

        public ReactiveCommand<IPackageSearchMetadata, IEnumerable<NuGetVersionViewModel>> GetVersions { get; set; }

        public IPackageSearchMetadata PackageSearchMetadata
        {
            get => _packageSearchMetadata;
            set => this.RaiseAndSetIfChanged(ref _packageSearchMetadata, value);
        }

        public IEnumerable<NuGetVersionViewModel> Versions => _versions.Value;

        public override IObservable<Unit> WhenNavigatedTo(INavigationParameter parameter)
        {
            if (parameter.TryGetValue("PackageMetadata", out var metadata))
            {
                PackageSearchMetadata = (IPackageSearchMetadata)metadata;
            }

            return base.WhenNavigatedTo(parameter);
        }

        private async Task<IEnumerable<NuGetVersionViewModel>> ExecuteGetVersions(IPackageSearchMetadata packageSearchMetadata)
        {
            var versions = await _nuGetPackageService.GetVersions(packageSearchMetadata);
            return versions.Reverse().Take(30).Select(x => new NuGetVersionViewModel(x));
        }
    }
}