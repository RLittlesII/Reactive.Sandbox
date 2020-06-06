using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Dialog.Main;
using DryIoc;
using ReactiveUI;
using Rocket.Surgery.Airframe.Composition;
using Rocket.Surgery.Airframe.Forms;
using Sextant;
using Sextant.XamForms;
using Xamarin.Forms;

namespace Dialog
{
    public partial class App : ApplicationBase
    {
        public App()
        {
            InitializeComponent();

            Container
                .Resolve<IParameterViewStackService>()
                .PushPage<MainViewModel>()
                .Subscribe();

            MainPage = (NavigationPage) Container.Resolve<IView>(nameof(NavigationView));
        }

        protected override void OnStart()
        {
            var pageAppearing = Application.Current.Events().PageAppearing.Where(x => x != null).Publish().RefCount();

            pageAppearing
                .Select(page => Interactions.ShowAlert.RegisterHandler(context => ShowAlert(context, page)))
                .Subscribe();

            pageAppearing
                .Select(page => Interactions.ShowActionSheet.RegisterHandler(context => ShowActionSheet(context, page)))
                .Subscribe();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        protected override void RegisterServices(IContainer container)
        {
            container
                .RegisterView<MainPage, MainViewModel>()
                .RegisterViewModel<MainViewModel>()
                .RegisterView<ConfirmPopup, ConfirmViewModel>()
                .RegisterViewModel<ConfirmViewModel>()
                .RegisterModule(new SextantModule());
        }

        private static async Task ShowAlert(InteractionContext<AlertDetail,bool> context, Page page)
        {
            await page.DisplayAlert(context.Input.Title, context.Input.Message, context.Input.Cancel);
            context.SetOutput(true);
        }

        private static async Task ShowActionSheet(InteractionContext<ActionSheetDetail,string> context, Page page)
        {
            var result = await page.DisplayActionSheet(context.Input.Title, context.Input.Cancel, context.Input.Destruction, context.Input.Buttons);
            context.SetOutput(result);
        }
    }
}
