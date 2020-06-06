using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using ReactiveUI;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Dialog.Alerts
{
    public class AlertViewModel : ReactiveObject
    {
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
                            .Select(x => confirmationPage.ViewModel.Result)
                        , (_, result) => result
                    )
                    .Subscribe(observer);
            });
    }
}