using System;
using DryIoc;
using NotificationCenter;
using ReactiveUI;
using Serilog;
using Sextant;
using Sextant.Abstractions;
using Splat;
using Splat.DryIoc;
using Splat.Serilog;
using UIKit;

namespace ListView
{
    public class Composition
    {
        private static IContainer _container;

        static Composition()
        {
            Locator.CurrentMutable.UseSerilogFullLogger();

            Log.Logger =
                new LoggerConfiguration()
                    .WriteTo
                    .NSLog()
                    .CreateLogger();

            _container = new Container(rules => rules.WithoutThrowOnRegisteringDisposableTransient());

            var navigationViewController = new NavigationViewController();
            _container.RegisterInstance<IView>(navigationViewController, serviceKey: "NavigationView");
            _container.RegisterInstance<IParameterViewStackService>(new ParameterViewStackService(navigationViewController));
            _container.Register<IViewModelFactory, DefaultViewModelFactory>();

            _container.Register<IViewFor<ListViewModel>, ListController>();
            _container.Register<ListViewModel>();
            _container.RegisterInstance<ItemDataService>(new ItemDataService());
            
            _container.UseDryIocDependencyResolver();
        }

        public static UIViewController StartPage()
        {
            _container.Resolve<IParameterViewStackService>().PushPage<ListViewModel>().Subscribe();
            
            return _container.Resolve<IView>("NavigationView") as UINavigationController;
        }
    }
}