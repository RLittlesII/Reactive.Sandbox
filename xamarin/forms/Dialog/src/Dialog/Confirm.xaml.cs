using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Rocket.Surgery.Airframe.Popup;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dialog
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfirmPopup : PopupPageBase<ConfirmViewModel>, IConfirmPage
    {
        public ConfirmPopup()
        {
            InitializeComponent();
            Observable
                .FromEvent<EventHandler, EventArgs>(eventHandler =>
                {
                    void Handler(object sender, EventArgs args) => eventHandler(args);
                    return Handler;
                },
                x => Disappearing += x,
                x => Disappearing -= x);

            Confirm
                .Events()
                .Clicked
                .InvokeCommand(this, x => x.ViewModel.ConfirmCommand);
        }

        public bool Result { get; set; }
    }

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
                        , (_, result) => result
                    )
                    .Subscribe(observer);
            });
    }

    public interface IConfirmPage
    {
        bool Result { get; set; }
    }
}