using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables;
using DynamicData;
using ReactiveUI;
using Sextant;

namespace ListView
{
    public class ListViewModel : ReactiveObject, IViewModel
    {
        private static readonly CompositeDisposable Registrations = new CompositeDisposable();
        private readonly ItemDataService _itemDataService;
        private ObservableCollection<TableCellViewModel> _items;
        private readonly ReadOnlyObservableCollection<TableCellViewModel> _someItems;
        private readonly ReadOnlyObservableCollection<TableCellViewModel> _otherItems;
        private readonly ReadOnlyObservableCollection<TableCellViewModel> _allItems;
        private TableCellViewModel _selectedItem;

        public ListViewModel(ItemDataService itemDataService)
        {
            _itemDataService = itemDataService;

            _itemDataService
                .ChangedItems
                .Transform(x => new TableCellViewModel(x))
                .Bind(out _allItems)
                .DisposeMany()
                .Subscribe()
                .DisposeWith(Registrations);

            _itemDataService
                .ChangedItems
                .Filter(x => x.Type == ItemType.Some)
                .Transform(x => new TableCellViewModel(x))
                .Bind(out _someItems)
                .DisposeMany()
                .Subscribe()
                .DisposeWith(Registrations);

            _itemDataService
                .ChangedItems
                .Filter(x => x.Type == ItemType.Other)
                .Transform(x => new TableCellViewModel(x))
                .Bind(out _otherItems)
                .DisposeMany()
                .Subscribe()
                .DisposeWith(Registrations);

            ChangeSegment = ReactiveCommand.Create<int, Unit>(segment =>
            {
                switch (segment)
                {
                    case 0:
                        Items = new ObservableCollection<TableCellViewModel>(_someItems);
                        break;
                    case 1:
                        Items = new ObservableCollection<TableCellViewModel>(_otherItems);
                        break;
                    case 2:
                        Items = new ObservableCollection<TableCellViewModel>(_allItems);
                        break;
                }

                return Unit.Default;
            });
        }

        public string Id { get; } = "List View";

        public ObservableCollection<TableCellViewModel> Items
        {
            get => _items;
            set => this.RaiseAndSetIfChanged(ref _items, value);
        }

        public TableCellViewModel SelectedItem
        {
            get => _selectedItem;
            set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
        }

        public ReactiveCommand<int, Unit> ChangeSegment { get; set; }
    }
}