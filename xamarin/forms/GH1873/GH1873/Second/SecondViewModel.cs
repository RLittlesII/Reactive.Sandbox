using System;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;

namespace GH1873
{
    public class SecondViewModel : RoutableViewModel
    {
        public ReactiveCommand<Unit, Unit> NavigateCommand { get; set; }

        public ReactiveCommand<Unit, Unit> NavigateBackCommand { get; set; }

        public SecondViewModel()
        {
            NavigateCommand = ReactiveCommand.CreateFromObservable(() => ExecuteNavigation());
            NavigateBackCommand = ReactiveCommand.CreateFromObservable(() => ExecuteBackNavigation());
        }

        private IObservable<Unit> ExecuteBackNavigation()
        {
            HostScreen.Router.NavigateBack.Execute().Subscribe();
            return Observable.Return(Unit.Default);
        }

        private IObservable<Unit> ExecuteNavigation()
        {
            HostScreen.Router.Navigate.Execute(new ThirdViewModel()).Subscribe();
            return Observable.Return(Unit.Default);
        }
    }
}