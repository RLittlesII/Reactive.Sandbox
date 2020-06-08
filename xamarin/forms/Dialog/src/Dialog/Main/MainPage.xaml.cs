using System;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Dialog.Actions;
using Dialog.Alerts;
using ReactiveUI;
using Rg.Plugins.Popup.Services;
using Rocket.Surgery.Airframe.Forms;
using Xamarin.Forms;


namespace Dialog.Main
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPageBase<MainViewModel>
    {
        public MainPage()
        {
            InitializeComponent();

            this.BindCommand(ViewModel, x => x.Alert, x => x.Alert, nameof(Alert.Clicked));
            this.BindCommand(ViewModel, x => x.ActionSheet, x => x.Action, nameof(Action.Clicked));
            this.BindCommand(ViewModel, x => x.Confirmation, x => x.Confirm, nameof(Confirm.Clicked));
            this.OneWayBind(ViewModel, x => x.Action, x => x.Output.Text);

            Interactions
                .ShowAlert
                .RegisterHandler(async context =>
                {
                    var alertPage = new Alert(context.Input);
                    
                    // Task.WhenAll();
                    var result =
                        await PopupNavigation
                            .Instance
                            .PushAsync(alertPage)
                            .ToObservable()
                            .ForkJoin(
                                alertPage
                                    .Events()
                                    .Disappearing
                                    .Take(1)
                                    .Select(x => Unit.Default),
                                (_, __) => __
                            );
                    context.SetOutput(result);
                });

            Interactions
                .ShowActionSheet
                .RegisterHandler(async context =>
                {
                    var actionsheet = new ActionSheet(context.Input);

                    // Task.WhenAll();
                    var result =
                        await PopupNavigation
                            .Instance
                            .PushAsync(actionsheet)
                            .ToObservable()
                            .ForkJoin(
                                actionsheet
                                    .Events()
                                    .Disappearing
                                    .Take(1)
                                    .Select(x => actionsheet.Result),
                                (_, result) => result);

                    context.SetOutput(result);
                });

            Interactions
                .ShowConfirmation
                .RegisterHandler(async context =>
                {
                    var confirmationPage = new ConfirmPopup();
                    
                    // Task.WhenAll();
                    var result =
                        await PopupNavigation
                                .Instance
                                .PushAsync(confirmationPage)
                                .ToObservable()
                                .ForkJoin(
                                    confirmationPage
                                            .Events()
                                            .Disappearing
                                            .Take(1)
                                            .Select(x => confirmationPage.Result),
                                    (_, result) => result
                    );
                    context.SetOutput(result);
                });

            async Task<bool> ConfirmAsync(string message, string title, string acceptText, string rejectText)
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
            this.WhenAnyValue(x => x.ViewModel)
                .Where(x => x != null)
                .Subscribe(viewModel =>
                {
                    viewModel
                        .ErrorInteraction
                        .RegisterHandler(async context =>
                        {
                            var result = DisplayAlert("Error", context.Input, "Ok");
                            context.SetOutput(true);
                        });
                });
        }
    }
}
