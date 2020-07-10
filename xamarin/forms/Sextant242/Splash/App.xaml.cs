using System;
using Splash.Splash;
using Splat;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Splash
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new Startup().NavigateToStart<SplashViewModel>();
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
}
