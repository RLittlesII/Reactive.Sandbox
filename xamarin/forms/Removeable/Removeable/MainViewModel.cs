using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using ReactiveUI;

namespace Removeable
{
    public class MainViewModel : ReactiveObject
    {
        private readonly ICoffeeDataService _coffeeDataService;
        private readonly ReadOnlyObservableCollection<MainItemViewModel> _items;

        CompositeDisposable _disposable = new CompositeDisposable();
        public MainViewModel(ICoffeeDataService coffeeDataService)
        {
            _coffeeDataService = coffeeDataService;
            _coffeeDataService
                .ChangeSet
                .Transform(x => new MainItemViewModel(coffeeDataService) { Id = x.Id, Title = x.Name })
                .AutoRefresh(x => x.Id)
                .DeferUntilLoaded()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _items)
                .Subscribe();

            coffeeDataService.Read().Subscribe();

            Add = ReactiveCommand.CreateFromObservable(ExecuteAdd);

            // ItemViewModels = new ObservableCollection<MainItemViewModel>();
            // var collectionChange = 
            //     Observable
            //         .FromEvent<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs> (eventHandler =>
            //     {
            //         void Handler(object sender, NotifyCollectionChangedEventArgs arg) => eventHandler(arg);
            //         return Handler;
            //     },x => ItemViewModels.CollectionChanged += x,
            //     x => ItemViewModels.CollectionChanged -= x);
            //
            // collectionChange
            //     .Throttle(TimeSpan.FromMilliseconds(300), RxApp.TaskpoolScheduler)
            //     .Where(x => x.NewItems.Count > 0)
            //     .Subscribe(_ => this.RaisePropertyChanged(nameof(ItemViewModels)))
            //     .DisposeWith(_disposable);
            //
            // ItemViewModels.CollectionChanged += ItemViewModelsOnCollectionChanged;
        }

        public ReactiveCommand<Unit, Unit> Add { get; set; }

        private IObservable<Unit> ExecuteAdd()
        {
            return Observable.Return(Unit.Default).Do(_ => _coffeeDataService.Create(new CoffeeDto()));
        }

        private void ItemViewModelsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Buffer
            // Timer
            // Check incoming events to cache
        }

        // public ObservableCollection<MainItemViewModel> ItemViewModels { get; set; }

        public ReadOnlyObservableCollection<MainItemViewModel> Items => _items;

        public void Dispose()
        {
            // ItemViewModels.CollectionChanged -= ItemViewModelsOnCollectionChanged;
        }
    }
}