using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Input;
using ReactiveUI;

namespace Dialog.Main
{
    public class MainViewModel : NavigationViewModelBase
    {
        private string _action;

        public MainViewModel()
            : base()
        {
            Alert = ReactiveCommand.CreateFromObservable(ExecuteAlert);

            ActionSheet = ReactiveCommand.CreateFromObservable(ExecuteActionSheet);

            Confirmation = ReactiveCommand.CreateFromObservable(ExecuteConfirm);

            ActionSheet.ThrownExceptions.Subscribe(exception => ErrorInteraction.Handle(exception.Message));
            Confirmation.ThrownExceptions.Subscribe(exception => ErrorInteraction.Handle(exception.Message));
        }

        public string Action
        {
            get => _action;
            set => this.RaiseAndSetIfChanged(ref _action, value);
        }

        public ReactiveCommand<Unit, Unit> ActionSheet { get; set; }

        public ReactiveCommand<Unit, Unit> Alert { get; set; }

        public ReactiveCommand<Unit, Unit> Confirmation { get; set; }

        protected override void ComposeObservables()
        {
        }

        protected override void RegisterObservers()
        {
        }

        private IObservable<Unit> ExecuteActionSheet()
        {
            Interactions
                .ShowActionSheet
                .Handle(new ActionSheetDetail("Attention", "Hello", "Goodbye", "ReactiveUI", "Pharmacist", "DynamicData"))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(output => Action = output);
            
            return Observable.Return(Unit.Default);
        }

        private IObservable<Unit> ExecuteAlert()
        {
            Interactions
                .ShowAlert
                .Handle(new AlertDetail("Attention", "Hello", "Goodbye"))
                .Subscribe();

            return Observable.Return(Unit.Default);
        }

        private IObservable<Unit> ExecuteConfirm() =>
            Interactions
                .ShowConfirmation
                .Handle(new ConfirmationDetail("Confirmation", "Hello"))
                .Select(x => Unit.Default);
    }
}