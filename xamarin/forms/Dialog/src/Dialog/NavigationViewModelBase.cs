using System;
using System.Reactive;
using System.Reactive.Linq;
using Rocket.Surgery.Airframe.ViewModels;
using Sextant;

namespace Dialog.Main
{
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