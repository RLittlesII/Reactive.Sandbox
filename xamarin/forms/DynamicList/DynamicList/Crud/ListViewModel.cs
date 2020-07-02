using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using Rocket.Surgery.Airframe.Synthetic;

namespace DynamicList.Crud
{
    public class ListViewModel : NavigableViewModel
    {
        private readonly IDrinkService _drinkDataService;
        private readonly ReadOnlyObservableCollection<ItemViewModel> _items;
        private readonly ObservableAsPropertyHelper<bool> _isRefreshing;
        private string _searchText;

        public ListViewModel(IDrinkService drinkDataService)
        {
            _drinkDataService = drinkDataService;

            this.WhenAnyValue(x => x.SearchText);

            _drinkDataService
                .ChangeSet
                .Transform(x => new ItemViewModel { Id = x.Id, Title = x.Title, Type = x.Type })
                .AutoRefresh(x => x.Id)
                .DeferUntilLoaded()
                .Sort(SortExpressionComparer<ItemViewModel>.Descending(x => x.Type).ThenByAscending(x => x.Id))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _items)
                .DisposeMany()
                .Subscribe()
                .DisposeWith(ViewModelSubscriptions);

            Add = ReactiveCommand.CreateFromObservable<EventArgs, Unit>(ExecuteAdd).DisposeWith(ViewModelSubscriptions);
            Refresh = ReactiveCommand.CreateFromObservable<EventArgs, Unit>(ExecuteRefresh).DisposeWith(ViewModelSubscriptions);
            Remove = ReactiveCommand.CreateFromObservable<ItemViewModel, Unit>(ExecuteRemove, Observable.Return(true)).DisposeWith(ViewModelSubscriptions);

            this.WhenAnyObservable(x => x.Refresh.IsExecuting)
                .StartWith(false)
                .DistinctUntilChanged()
                .ToProperty(this, nameof(IsRefreshing), out _isRefreshing)
                .DisposeWith(ViewModelSubscriptions);
        }

        public string Id { get; }

        public bool IsRefreshing => _isRefreshing.Value;

        public ReactiveCommand<EventArgs, Unit> Add { get; set; }

        public ReactiveCommand<EventArgs, Unit> Refresh { get; set; }

        public ReactiveCommand<ItemViewModel, Unit> Remove { get; set; }

        public ReadOnlyObservableCollection<ItemViewModel> Items => _items;

        public string SearchText
        {
            get => _searchText;
            set => this.RaiseAndSetIfChanged(ref _searchText, value);
        }

        private IObservable<Unit> ExecuteAdd(EventArgs args) =>
            Observable.Create<Unit>(observer => Interactions.AddItem.Handle(Unit.Default).ObserveOn(RxApp.MainThreadScheduler).Subscribe(observer).DisposeWith(ViewModelSubscriptions));

        private IObservable<Unit> ExecuteRefresh(EventArgs args) => Observable.Create<Unit>(observer =>
            _drinkDataService
                .Read()
                .Select(x => Unit.Default)
                .Delay(TimeSpan.FromSeconds(2), RxApp.TaskpoolScheduler)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(observer)
                .DisposeWith(ViewModelSubscriptions));

        private IObservable<Unit> ExecuteRemove(ItemViewModel item) => _drinkDataService.Delete(item.Id);
    }
}