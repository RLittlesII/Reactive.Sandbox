using System;
using System.Reactive.Linq;
using Dialog.Main;
using DryIoc;
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
                .RegisterModule(new SextantModule());
        }
    }
}
