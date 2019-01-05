using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;

namespace GH1873
{
    public class ThirdViewModel : RoutableViewModel
    {
        public ReactiveCommand<Unit, Unit> NavigateAndResetCommand { get; set; }

        public ThirdViewModel()
        {
            NavigateAndResetCommand = ReactiveCommand.CreateFromObservable(() => ExecuteNavigateAndReset());

            HostScreen.Router.NavigateAndReset.ThrownExceptions.Subscribe(_ => Debug.Write(_.Message));
        }

        private IObservable<Unit> ExecuteNavigateAndReset()
        {
            HostScreen.Router.NavigateAndReset.Execute(new FirstViewModel()).Subscribe();
            return Observable.Return(Unit.Default);
        }
    }
}