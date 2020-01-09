using System;
using ReactiveUI;
using Splat;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Mobile
{
    /// <summary>
    /// The main application instance.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            InitializeComponent();

            var bootstrapper = new AppBootstrapper();
            MainPage = AppBootstrapper.CreateMainPage();
        }

        /// <inheritdoc/>
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
}


























