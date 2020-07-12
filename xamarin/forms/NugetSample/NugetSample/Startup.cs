using System;
using NugetSample.Nuget;
using NugetSample.Services;
using ReactiveUI;
using Sextant;
using Sextant.Abstractions;
using Sextant.XamForms;
using Splat;
using Xamarin.Forms;

namespace NugetSample
{
    public class Startup
    {
        public Startup(IDependencyResolver dependencyResolver = null)
        {
            if (dependencyResolver == null)
            {
                dependencyResolver = new ModernDependencyResolver();
            }

            RxApp.DefaultExceptionHandler = new NugetSampleExceptionHandler();
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
            dependencyResolver.RegisterView<NuGetPackageListView, NuGetPackageListViewModel>();
            dependencyResolver.RegisterView<NugetPackageDetailPage, NuGetPackageDetailViewModel>();
        }

        private void RegisterViewModels(IDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterViewModel<MainViewModel>();
            dependencyResolver.RegisterViewModel<NuGetPackageListViewModel>(() => new NuGetPackageListViewModel(dependencyResolver.GetService<IParameterViewStackService>(), dependencyResolver.GetService<INuGetPackageService>()));
            dependencyResolver.RegisterViewModel<NuGetPackageDetailViewModel>(() => new NuGetPackageDetailViewModel(dependencyResolver.GetService<INuGetPackageService>()));
        }

        private void RegisterServices(IDependencyResolver dependencyResolver)
        {
            var navigationView = new NavigationView();
            dependencyResolver.RegisterNavigationView(() => navigationView);
            dependencyResolver.RegisterLazySingleton<IParameterViewStackService>(() => new ParameterViewStackService(navigationView));
            dependencyResolver.RegisterLazySingleton<IViewModelFactory>(() => new DefaultViewModelFactory());
            dependencyResolver.RegisterLazySingleton<INuGetPackageService>(() => new NuGetPackageService());
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