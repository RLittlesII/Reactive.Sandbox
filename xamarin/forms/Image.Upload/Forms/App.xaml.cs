﻿using System;
using System.Reactive.Linq;
using Forms.Data;
using Forms.Explorer;
using Forms.Services;
using ReactiveUI;
using Sextant;
using Sextant.XamForms;
using Splat;
using Xamarin.Forms;
using Akavache;
using System.Linq;
using System.Reactive;

namespace Forms
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Make sure you set the application name before doing any inserts or gets
            Akavache.Registrations.Start("UploadManager");

            RxApp.DefaultExceptionHandler = new ExceptionHandler();

            Sextant.Sextant.Instance.InitializeForms();

            Locator
                .CurrentMutable
                .RegisterLazySingleton(() => new Cache(), typeof(ICache));
            Locator
                .CurrentMutable
                .RegisterLazySingleton(() => new FormsService(), typeof(IFormsService));

            Locator
                .CurrentMutable
                .RegisterView<FormsToUploadPage, FormsToUploadPageViewModel>();

            Locator
                .Current
                .GetService<IParameterViewStackService>()
                .PushPage(new FormsToUploadPageViewModel(new UploadService()), resetStack: true, animate: false)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe();

            MainPage = Locator.Current.GetNavigationView("NavigationView");
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            var caches = new[]
            {
                BlobCache.LocalMachine,
                BlobCache.Secure,
                BlobCache.UserAccount,
                BlobCache.InMemory
            };

            caches.Select(x => x.Flush())
                  .Merge()
                  .ToSignal()
                  .Wait();

            BlobCache.Shutdown().Wait();
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
    
    public class ExceptionHandler : IObserver<Exception>
    {
        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(Exception value)
        {
        }
    }
}
