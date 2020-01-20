﻿using System;
using System.Reactive.Linq;
using Forms.Explorer;
using Forms.Services;
using ReactiveUI;
using Sextant;
using Sextant.XamForms;
using Splat;
using Xamarin.Forms;

namespace Forms
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            
            Sextant.Sextant.Instance.InitializeForms();

            Locator
                .CurrentMutable
                .RegisterView<AkavacheExplorer, AkavacheExplorerViewModel>();

            Locator
                .Current
                .GetService<IParameterViewStackService>()
                .PushPage(new AkavacheExplorerViewModel(new UploadService()), resetStack: true, animate: false)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe();

            MainPage = Locator.Current.GetNavigationView();
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
}
