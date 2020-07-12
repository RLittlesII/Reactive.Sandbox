using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using NugetSample.Services;
using ReactiveUI;
using Sextant;
using Splat;

namespace NugetSample.Nuget
{
    public class NuGetPackageListViewModel : NavigableVieModel
    {
        private readonly IParameterViewStackService _navigationService;
        private readonly INuGetPackageService _nugetPackageService;
        private string _searchText;
        private ObservableAsPropertyHelper<IEnumerable<NuGetPackageViewModel>> _searchResults;
        private ObservableAsPropertyHelper<bool> _isRefreshing;
        private ObservableAsPropertyHelper<bool> _hasItems;

        public NuGetPackageListViewModel(IParameterViewStackService navigationService, INuGetPackageService nugetPackageService)
        {
            _navigationService = navigationService;
            _nugetPackageService = nugetPackageService;

            PackageDetails = ReactiveCommand.CreateFromTask<NuGetPackageViewModel>(ExecutePackageDetails);
            Refresh = ReactiveCommand.CreateFromTask(ExecuteRefresh);

            Refresh.ThrownExceptions.Subscribe(exception => this.Log().Warn<Exception>(exception));
            PackageDetails.ThrownExceptions.Subscribe(exception => this.Log().Warn(exception));

            _searchResults =
                this.WhenAnyValue(x => x.SearchText)
                    .Throttle(TimeSpan.FromMilliseconds(800))
                    .Select(term => term?.Trim())
                    .DistinctUntilChanged()
                    .SelectMany(SearchNuGetPackages)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .ToProperty(this, x => x.SearchResults);

            _isRefreshing =
                this.WhenAnyObservable(x => x.Refresh.IsExecuting)
                    .StartWith(false)
                    .DistinctUntilChanged()
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .ToProperty(this, x => x.IsRefreshing);

            _hasItems =
                this.WhenAnyValue(x => x.SearchResults, x => x != null && x.Any())
                    .ToProperty(this, x => x.HasItems);
        }

        public bool IsRefreshing => _isRefreshing.Value;

        public bool HasItems => _hasItems.Value;

        public string SearchText
        {
            get => _searchText;
            set => this.RaiseAndSetIfChanged(ref _searchText, value);
        }

        public string Instructions = "Search for a NuGet package.";

        public IEnumerable<NuGetPackageViewModel> SearchResults => _searchResults.Value;

        public ReactiveCommand<NuGetPackageViewModel, Unit> PackageDetails { get; set; }

        public ReactiveCommand<Unit, Unit> Refresh { get; set; }

        private async Task ExecutePackageDetails(NuGetPackageViewModel viewModel) =>
            await _navigationService.PushPage<NuGetPackageDetailViewModel>(
                new NavigationParameter
                {
                    { "PackageMetadata", viewModel.PackageMetadata }
                });

        private async Task<IEnumerable<NuGetPackageViewModel>> SearchNuGetPackages(string term, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return Enumerable.Empty<NuGetPackageViewModel>();
            }

            var result = await _nugetPackageService.SearchNuGetPackages(term, token);
            return result.Select(x => new NuGetPackageViewModel(x));
        }

        private async Task ExecuteRefresh()
        {
            var result = await _nugetPackageService.SearchNuGetPackages(SearchText, CancellationToken.None);
        }
    }
}