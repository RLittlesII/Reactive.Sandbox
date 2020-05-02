using System;
using ReactiveUI;
using Sextant;
using Sextant.XamForms;
using Splat;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Sextant.Sextant;

namespace Sextant202
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            RxApp.DefaultExceptionHandler = new NavigationExceptionHandler();

            Instance.InitializeForms();

            Locator
                .CurrentMutable
                .RegisterView<MainPage, MainPageViewModel>();

            MainPage = new MainPage();
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
    }

    public class NavigationExceptionHandler : IObserver<Exception>
    {
        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(Exception value) { }
    }
}
