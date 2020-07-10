using System;
using ReactiveUI;
using Sextant;
using Sextant.Abstractions;
using Sextant.XamForms;
using Splash.Login;
using Splash.Splash;
using Splat;
using Xamarin.Forms;

namespace Splash
{
    public class Startup
    {
        public Startup(IDependencyResolver dependencyResolver = null)
        {
            if (dependencyResolver == null)
            {
                dependencyResolver = new ModernDependencyResolver();
            }

            RxApp.DefaultExceptionHandler = new SplashExceptionHandler();
            RegisterServices(dependencyResolver);
            RegisterViewModels(dependencyResolver);
            RegisterViews(dependencyResolver);
            Build(dependencyResolver);
        }

        private void Build(IDependencyResolver dependencyResolver) =>
            Locator.SetLocator(dependencyResolver);

        private void RegisterViews(IDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterView<MainPage, MainViewModel>();
            dependencyResolver.RegisterView<LoginPage, LoginViewModel>();
            dependencyResolver.RegisterView<SplashPage, SplashViewModel>();
        }

        private void RegisterViewModels(IDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterViewModel<MainViewModel>();
            dependencyResolver.RegisterViewModel<LoginViewModel>();
            dependencyResolver.RegisterViewModel<SplashViewModel>();
        }

        private void RegisterServices(IDependencyResolver dependencyResolver)
        {
            var navigationView = new NavigationView();
            dependencyResolver.RegisterNavigationView(() => navigationView);
            dependencyResolver.RegisterLazySingleton<IParameterViewStackService>(() => new ParameterViewStackService(navigationView));
            dependencyResolver.RegisterLazySingleton<IViewModelFactory>(() => new DefaultViewModelFactory());
            dependencyResolver.InitializeReactiveUI();
        }

        public Page NavigateToStart<T>()
            where T : IViewModel
        {
            Locator.Current.GetService<IParameterViewStackService>().PushPage<T>().Subscribe();
            return Locator.Current.GetNavigationView(nameof(NavigationView));
        }
    }
}