using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using Splat;

namespace Routing.Tests
{
    public abstract class ItemViewModel : ReactiveObject, IRoutableViewModel
    {
        public ReactiveCommand<Unit, Unit> Confirm { get; }
        public ReactiveCommand<Unit, Unit> Cancel { get; }
        public ReactiveCommand<Unit, Unit> Delete { get; }

        protected IRoutableViewModel NextViewModel { get; set; } = null;

        public abstract string UrlPathSegment { get; }
        public IScreen HostScreen { get; }

        public abstract int Id { get; }
        public abstract string Name { get; }

        public ItemViewModel(IScreen screen = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>(); // IRoutableViewModel implementation.

            Confirm = ReactiveCommand.CreateFromObservable(OnConfirm);
            Cancel = ReactiveCommand.CreateFromObservable(OnCancel);
            Delete = ReactiveCommand.CreateFromObservable(OnDelete);

            // completedInternally emits when either of Confirm, Cancel or Delete emits and completes.
            var completedInternally = Observable.Amb(
                Confirm.Select(_ => _),
                Cancel.Select(_ => _),
                Delete.Select(_ => _));

            completedInternally.Subscribe(_ =>
            {
                if (HostScreen.Router.NavigationStack.LastOrDefault() == this)
                {
                    HostScreen.Router.NavigateBack.Execute().Subscribe(); // Return to the calling ViewModel when completed.

                    if (NextViewModel != null)
                        HostScreen.Router.Navigate.Execute(NextViewModel).Subscribe();
                }
            });
        }

        /// <summary>
        /// Executes on Confirm command.
        /// </summary>
        private IObservable<Unit> OnConfirm()
        {
            OnConfirmIfNew();

            return Observable.Return(Unit.Default);
        }

        /// <summary>
        /// Executes on Confirm command if the item is new.
        /// </summary>
        protected abstract void OnConfirmIfNew();

        /// <summary>
        /// Executes on Cancel command.
        /// </summary>
        private IObservable<Unit> OnCancel()
        {
            OnCancelIfNew();

            return Observable.Return(Unit.Default);
        }

        /// <summary>
        /// Executes on Cancel command if the item is new.
        /// </summary>
        protected virtual void OnCancelIfNew() { }

        /// <summary>
        /// Executes on Delete command.
        /// </summary>
        private IObservable<Unit> OnDelete()
        {
            OnDeleteIfNew();

            return Observable.Return(Unit.Default);
        }

        /// <summary>
        /// Executes on Confirm command if the item is new.
        /// </summary>
        protected virtual void OnDeleteIfNew() { }
    }
}