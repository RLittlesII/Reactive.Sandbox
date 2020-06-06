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
        public IObservable<bool> Result { get; }
        public ReactiveCommandBase<EventArgs, Unit> ConfirmCommand { get; set; }

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
    }
}