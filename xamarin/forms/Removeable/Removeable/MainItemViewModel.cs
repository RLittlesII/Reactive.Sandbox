using System;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;

namespace Removeable
{
    public class MainItemViewModel : ReactiveObject
    {
        private readonly ICoffeeDataService _coffeeDataService;
        private Guid _id;
        private string _title;

        public MainItemViewModel(ICoffeeDataService coffeeDataService)
        {
            _coffeeDataService = coffeeDataService;
            Remove = ReactiveCommand.CreateFromObservable(ExecuteRemove);
        }

        public ReactiveCommand<Unit, Unit> Remove { get; set; }

        private IObservable<Unit> ExecuteRemove() => Observable.Return(Unit.Default).Do(_ =>_coffeeDataService.Delete(Id).Subscribe());

        public Guid Id
        {
            get => _id;
            set => this.RaiseAndSetIfChanged(ref _id, value);
        }

        public string Title
        {
            get => _title;
            set => this.RaiseAndSetIfChanged(ref _title, value);
        }
    }
}