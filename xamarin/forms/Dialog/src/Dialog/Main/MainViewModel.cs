using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Input;
using ReactiveUI;
using Rocket.Surgery.Airframe.ViewModels;
using Sextant;

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

            ActionSheet.ThrownExceptions.Subscribe(exception => ErrorInteraction.Handle(exception.Message));
        }

        public string Action
        {
            get => _action;
            set => this.RaiseAndSetIfChanged(ref _action, value);
        }

        public ReactiveCommand<Unit, Unit> ActionSheet { get; set; }

        public ReactiveCommand<Unit, Unit> Alert { get; set; }

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
                .Handle(new ActionSheetDetail("Attention", "Hello", "Goodbye", "ReactiveUI", "Pharmacist",
                    "DynamicData"))
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
    }

    public abstract class NavigationViewModelBase : ViewModelBase, INavigable
    {
        public virtual IObservable<Unit> WhenNavigatedTo(INavigationParameter parameter) =>
            Observable.Return(Unit.Default);

        public IObservable<Unit> WhenNavigatedFrom(INavigationParameter parameter) =>
            Observable.Return(Unit.Default);

        public IObservable<Unit> WhenNavigatingTo(INavigationParameter parameter) =>
            Observable.Return(Unit.Default);
    }
}