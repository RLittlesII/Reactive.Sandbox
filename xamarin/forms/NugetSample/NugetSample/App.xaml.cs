using System;
using NugetSample.Nuget;
using Splat;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NugetSample
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var startup = new Startup();
            MainPage = startup.NavigateToStart<NuGetPackageListViewModel>();
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
