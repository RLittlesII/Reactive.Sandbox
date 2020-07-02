using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using Sextant;

namespace DynamicList
{
    public abstract class NavigableViewModel : ReactiveObject, INavigable, IDestructible
    {
        protected readonly CompositeDisposable ViewModelSubscriptions = new CompositeDisposable();

        public string Id { get; }

        protected virtual IObservable<Unit> WhenNavigatedTo(INavigationParameter parameter) => 
            Observable.Create<Unit>(observer => Disposable.Empty);

        protected virtual IObservable<Unit> WhenNavigatedFrom(INavigationParameter parameter) => 
            Observable.Create<Unit>(observer => Disposable.Empty);

        protected virtual IObservable<Unit> WhenNavigatingTo(INavigationParameter parameter) =>
            Observable.Create<Unit>(observer => Disposable.Empty);

        IObservable<Unit> INavigated.WhenNavigatedTo(INavigationParameter parameter) => WhenNavigatedTo(parameter);

        IObservable<Unit> INavigated.WhenNavigatedFrom(INavigationParameter parameter) => WhenNavigatedFrom(parameter);

        IObservable<Unit> INavigating.WhenNavigatingTo(INavigationParameter parameter) => WhenNavigatingTo(parameter);

        void IDestructible.Destroy()
        {
            ViewModelSubscriptions?.Dispose();
        }
    }
}