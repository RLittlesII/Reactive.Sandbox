using System;
using ReactiveUI;
using ReactiveUI.XamForms;
using Splat;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Splat203
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppBootstrapper().NavigateToStart();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }

    public class AppBootstrapper : IScreen
    {
        public RoutingState Router { get; }

        public AppBootstrapper()
        {
            Locator.CurrentMutable.RegisterConstant(this, typeof(IScreen));
            Locator.CurrentMutable.Register(() => new MainViewModel());
            Locator.CurrentMutable.Register(() => new MainPage(), typeof(IViewFor<MainViewModel>));
            Locator.CurrentMutable.InitializeReactiveUI();
            Locator.CurrentMutable.InitializeSplat();

            Router = new RoutingState();

            Router.NavigateAndReset.Execute(new MainViewModel());
        }

        public RoutedViewHost NavigateToStart() => new RoutedViewHost();
    }
}
