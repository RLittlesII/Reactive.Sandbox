using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using DynamicData;
using ReactiveUI;

namespace Removeable
{
    public class MainViewModel : ReactiveObject
    {
        private readonly ReadOnlyObservableCollection<MainItemViewModel> _items;

        public MainViewModel(ICoffeeDataService coffeeDataService)
        {
            coffeeDataService
                .ChangeSet
                .Transform(x => new MainItemViewModel(coffeeDataService) { Id = x.Id, Title = x.Name })
                .AutoRefresh(x => x.Id)
                .DeferUntilLoaded()
                .Bind(out _items)
                .Subscribe();

            coffeeDataService.Read().Subscribe();
        }

        public ReadOnlyObservableCollection<MainItemViewModel> Items => _items;
    }
}