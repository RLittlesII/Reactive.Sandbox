using System;
using Splat;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReactiveUI2466
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new Startup().NavigateToStart<MainViewModel>();
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
