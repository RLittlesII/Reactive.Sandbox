using System;
using DryIoc;
using ReactiveUI;
using Sextant;
using Sextant.XamForms;
using Splat;
using Splat.DryIoc;
using Xamarin.Forms;

namespace Forms
{
    public class CompositionRoot
    {
        private static IContainer _container;
        static CompositionRoot()
        {
            _container = new Container();
            _container.Register<IViewFor<OneViewModel>, One>();
            _container.Register<IViewFor<TwoViewModel>, Two>();

            var navigationView = new NavigationView();
            _container.RegisterInstance<IView>(navigationView, IfAlreadyRegistered.Replace, Setup.Default, nameof(NavigationView));
            _container.RegisterInstance<IParameterViewStackService>(new ParameterViewStackService(navigationView));
            _container.UseDryIocDependencyResolver();
        }

        public static IContainer Root => _container;
        
        public static Page StartPage()
        {
            Locator.Current.GetService<IParameterViewStackService>().PushPage(new OneViewModel()).Subscribe();
            return Locator.Current.GetNavigationView(nameof(NavigationView));
        }
    }
}