using ReactiveUI;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Linq;

namespace GH1873
{
    public class FirstViewModel : RoutableViewModel
    {
        public ReactiveCommand<Unit, Unit> NavigateCommand { get; set; }

        public FirstViewModel()
        {
            NavigateCommand = ReactiveCommand.CreateFromObservable(() => ExecuteNavigation());
        }

        private IObservable<Unit> ExecuteNavigation()
        {
            HostScreen.Router.Navigate.Execute(new SecondViewModel()).Subscribe();
            return Observable.Return(Unit.Default);
        }
    }

    public class RoutableViewModel : ReactiveObject, IRoutableViewModel
    {
        public IScreen HostScreen { get; }
        public string UrlPathSegment { get; }

        public RoutableViewModel(IScreen screen = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();
        }
    }
}