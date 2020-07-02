using System;
using Data;
using DynamicList.Crud;
using ReactiveUI;
using Refit;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Services;
using Rocket.Surgery.Airframe.Synthetic;
using Sextant;
using Sextant.Abstractions;
using Sextant.XamForms;
using Splat;
using Xamarin.Forms;

namespace DynamicList
{
    public class Startup
    {
        public Startup(IDependencyResolver dependencyResolver = null)
        {
            if (dependencyResolver == null)
            {
                dependencyResolver = new ModernDependencyResolver();
            }

            RxApp.DefaultExceptionHandler = new DynamicListExceptionHandler();
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
            dependencyResolver.RegisterView<List, ListViewModel>();
            dependencyResolver.RegisterView<NewItem, NewItemViewModel>(() => new NewItem(dependencyResolver.GetService<NewItemViewModel>()));
        }

        private void RegisterViewModels(IDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterViewModel<MainViewModel>();
            dependencyResolver.RegisterViewModel(() => new ListViewModel(dependencyResolver.GetService<IDrinkService>()));
            dependencyResolver.RegisterViewModel(() => new NewItemViewModel(dependencyResolver.GetService<IPopupNavigation>(), dependencyResolver.GetService<IDrinkService>()));
        }

        private void RegisterServices(IDependencyResolver dependencyResolver)
        {
            var navigationView = new NavigationView();
            dependencyResolver.RegisterNavigationView(() => navigationView);
            dependencyResolver.RegisterLazySingleton<IParameterViewStackService>(() => new ParameterViewStackService(navigationView));
            dependencyResolver.RegisterLazySingleton<IViewModelFactory>(() => new DefaultViewModelFactory());
            
            dependencyResolver.RegisterLazySingleton<ICoffeeDataService>(() => new CoffeeDataService(new CoffeeClientMock()));
            dependencyResolver.RegisterLazySingleton<IDrinkService>(() => new DrinkDataService(new DrinkClientMock()));
            dependencyResolver.RegisterLazySingleton(() => RestService.For<IDuckDuckGoApi>("https://api.duckduckgo.com"));
            dependencyResolver.RegisterLazySingleton<IDuckDuckGoService>(() => new DuckDuckGoService(dependencyResolver.GetService<IDuckDuckGoApi>()));
            dependencyResolver.RegisterLazySingleton<IPopupNavigation>(() => PopupNavigation.Instance);
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