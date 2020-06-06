using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using ReactiveUI;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Dialog
{
    public class ConfirmViewModel : ReactiveObject
    {
        private ObservableAsPropertyHelper<bool> _result;
        private string _message;

        public ConfirmViewModel()
        {
            Confirm = ReactiveCommand.CreateFromObservable<EventArgs, Unit>(ExecuteConfirm);
            Cancel = ReactiveCommand.CreateFromObservable<EventArgs, Unit>(ExecuteCancel);

            // Result can be an observable of confirm/cancel execution.  Return truth from whichever ticks.

            _result =
                this.WhenAnyObservable(
                        x => x.Confirm.IsExecuting.DistinctUntilChanged(),
                        x => x.Cancel.IsExecuting.DistinctUntilChanged(),
                        (confirm, cancel) => confirm && !cancel)
                    .ToProperty(this, x => x.Result, false);
        }

        public bool Result => _result.Value;

        public ReactiveCommand<EventArgs, Unit> Confirm { get; set; }

        public ReactiveCommand<EventArgs, Unit> Cancel { get; set; }

        public string Message
        {
            get => _message;
            set => this.RaiseAndSetIfChanged(ref _message, value);
        }

        private IObservable<Unit> ExecuteConfirm(EventArgs args) => PopupNavigation.Instance.PopAsync().ToObservable();

        private IObservable<Unit> ExecuteCancel(EventArgs args) => PopupNavigation.Instance.PopAsync().ToObservable();

        /*
        public async Task<bool> ConfirmAsync(string message, string title, string acceptText, string rejectText)
        {
            var confirmationPage = new ConfirmPopup();
            var disappearing = new TaskCompletionSource<bool>();
            confirmationPage.Disappearing += OnDisappearing;
            await PopupNavigation.Instance.PushAsync(confirmationPage);
            return await disappearing.Task;

            void OnDisappearing(object sender, System.EventArgs e)
            {
                confirmationPage.Disappearing -= OnDisappearing;
                disappearing.SetResult(confirmationPage.Result); 
            }
        }

        public IObservable<bool> Confirm(string message, string title, string acceptText, string rejectText) =>
            Observable.Create<bool>(observer =>
            {
                var confirmationPage = new ConfirmPopup();
                // Task.WhenAll();
                return PopupNavigation
                    .Instance
                    .PushAsync(confirmationPage)
                    .ToObservable()
                    .ForkJoin(
                        confirmationPage
                            .Events()
                            .Disappearing
                            .Take(1)
                            .Select(x => confirmationPage.Result)
                        , (_, result) => result)
                    .Subscribe(observer);
            });
*/
    }
}