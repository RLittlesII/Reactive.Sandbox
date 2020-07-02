using System;
using DynamicList.Crud;
using Splat;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DynamicList
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new Startup().NavigateToStart<ListViewModel>();
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
