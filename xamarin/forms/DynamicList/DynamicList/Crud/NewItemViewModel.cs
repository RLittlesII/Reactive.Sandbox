using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using ReactiveUI;
using Rg.Plugins.Popup.Contracts;
using Rocket.Surgery.Airframe.Synthetic;

namespace DynamicList.Crud
{
    public class NewItemViewModel : NavigableViewModel
    {
        private readonly IPopupNavigation _popupNavigation;
        private readonly IDrinkService _drinkService;

        private DrinkType _selectedType;
        private string _title;

        public NewItemViewModel(IPopupNavigation popupNavigation, IDrinkService drinkService)
        {
            _popupNavigation = popupNavigation;
            _drinkService = drinkService;

            Save = ReactiveCommand.CreateFromObservable(ExecuteSave).DisposeWith(ViewModelSubscriptions);
            Cancel = ReactiveCommand.CreateFromObservable(ExecuteCancel).DisposeWith(ViewModelSubscriptions);
        }

        public ReactiveCommand<Unit, Unit> Save { get; set; }

        public ReactiveCommand<Unit, Unit> Cancel { get; set; }

        public string Title
        {
            get => _title;
            set => this.RaiseAndSetIfChanged(ref _title, value);
        }
        
        public DrinkType SelectedType
        {
            get => _selectedType;
            set => this.RaiseAndSetIfChanged(ref _selectedType, value);
        }

        public List<string> DrinkTypes => Enum.GetNames(typeof(DrinkType)).Select(b => b.SplitCamelCase()).ToList();

        private IObservable<Unit> ExecuteSave() => Observable.Create<Unit>(observer =>
        {
            _drinkService.Create(new DrinkDto {Title = Title, Type = SelectedType}).Subscribe(observer);
            return _popupNavigation.PopAsync().ToObservable().Subscribe(observer);
        });

        private IObservable<Unit> ExecuteCancel() =>
            _popupNavigation.PopAsync().ToObservable();
    }
}