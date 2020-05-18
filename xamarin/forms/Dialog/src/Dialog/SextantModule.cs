using DryIoc;
using Rocket.Surgery.Airframe.Composition;
using Sextant;
using Sextant.Abstractions;
using Sextant.XamForms;

namespace Dialog
{
    public class SextantModule : DryIocModule
    {
        public override void Load(IRegistrator registrar)
        {
            var navigationView = new NavigationView();
            registrar.RegisterInstance<IView>(navigationView, IfAlreadyRegistered.Replace, Setup.Default, nameof(NavigationView));
            registrar.RegisterInstance<IParameterViewStackService>(new ParameterViewStackService(navigationView));
            registrar.RegisterInstance<IViewModelFactory>(new DefaultViewModelFactory());
        }
    }
}