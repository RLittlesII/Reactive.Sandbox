using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Data;
using DynamicData;
using ReactiveUI;

namespace DynamicList.Search
{
    public class SearchListViewModel : NavigableViewModel
    {
        private readonly IDuckDuckGoService _duckDuckGoService;
        private string _searchText;
        private IEnumerable<object> _searchResults;
        private ReadOnlyObservableCollection<SearchResultViewModel> _searchItems;
        private ObservableAsPropertyHelper<bool> _isRefreshing;
        private ObservableAsPropertyHelper<SearchResultViewModel> _currentResult;

        public SearchListViewModel(IDuckDuckGoService duckDuckGoService)
        {
            _duckDuckGoService = duckDuckGoService;

            Search = ReactiveCommand.CreateFromTask<string>(ExecuteSearch);

            Refresh = ReactiveCommand.CreateFromTask(ExecuteRefresh);

            // this.WhenAnyValue(x => x.SearchText)
            //     .Throttle(TimeSpan.FromMilliseconds(750))
            //     .Where(x => !string.IsNullOrWhiteSpace(x))
            //     .DistinctUntilChanged()
            //     .InvokeCommand(this, x => x.Search);

            _duckDuckGoService
                .QueryResults
                .Transform(x => new SearchResultViewModel { Url = x.FirstUrl, DeepLink = x.Result })
                .DeferUntilLoaded()
                .Bind(out _searchItems)
                .DisposeMany()
                .Subscribe();

            _isRefreshing =
                this.WhenAnyObservable(x => x.Refresh.IsExecuting)
                    .StartWith(false)
                    .DistinctUntilChanged()
                    .ToProperty(this, x => x.IsRefreshing);
        }

        public string SearchText
        {
            get => _searchText;
            set => this.RaiseAndSetIfChanged(ref _searchText, value);
        }

        public ReactiveCommand<Unit, Unit> Refresh { get; set; }

        public ReactiveCommand<string, Unit> Search { get; set; }

        public bool IsRefreshing => _isRefreshing.Value;

        public SearchResultViewModel CurrentResult => _currentResult.Value;

        public ReadOnlyObservableCollection<SearchResultViewModel> SearchResults => _searchItems;

        private async Task ExecuteSearch(string searchText)
        {
            await _duckDuckGoService.Query(searchText);
        }

        private Task ExecuteRefresh()
        {
            return Observable.Return(Unit.Default).Delay(TimeSpan.FromSeconds(3)).ToTask();
        }
    }
}
