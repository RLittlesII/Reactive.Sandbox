using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using ImTools;
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
                    await DisplayAlert(context.Input.Title, context.Input.Message, context.Input.Cancel);
                    context.SetOutput(true);
                });

            Interactions
                .ShowActionSheet
                .RegisterHandler(async context =>
                {
                    var result = await DisplayActionSheet(context.Input.Title, context.Input.Cancel, context.Input.Destruction, context.Input.Buttons);
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
                    context.SetOutput(result.ToString());
                });

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
