using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LoadData
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new LoadData();
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
