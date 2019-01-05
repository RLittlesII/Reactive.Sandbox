using ReactiveUI;
using Splat;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace GH1873
{
    public static class Registrations
    {
        static Registrations()
        {
            RxApp.DefaultExceptionHandler = RxApp.DefaultExceptionHandler;
        }

        public static void Register(IMutableDependencyResolver container)
        {
            RegisterViews(container);
            RegisterScreen(container);
        }

        private static void RegisterScreen(IMutableDependencyResolver container)
        {
            container.RegisterConstant(new Screen(), typeof(IScreen));
        }

        private static void RegisterViews(IMutableDependencyResolver container)
        {
            container.RegisterViewsForViewModels(Assembly.GetExecutingAssembly());
        }
    }

    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Registrations.Register(Locator.CurrentMutable);
            var screen = new Screen();
            MainPage = screen.PresentDefaultView();
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }
    }
}