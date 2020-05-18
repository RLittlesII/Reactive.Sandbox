using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using ReactiveUI;
using Rocket.Surgery.Airframe.Forms;


namespace Dialog.Main
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPageBase<MainViewModel>
    {
        public MainPage()
        {
            InitializeComponent();

            this.BindCommand(ViewModel, x => x.Alert, x => x.Interact, nameof(Interact.Clicked));
            this.BindCommand(ViewModel, x => x.ActionSheet, x => x.Action, nameof(Action.Clicked));
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
