using ReactiveUI;
using ReactiveUI.XamForms;
using Splat;
using Xamarin.Forms;

namespace Forms
{
    public class AppBootstrapper : ReactiveObject, IScreen
    {
        public AppBootstrapper(IMutableDependencyResolver dependencyResolver = null, RoutingState router = null)
        {
            Router = router ?? new RoutingState();
            //
            RegisterParts(dependencyResolver ?? Locator.CurrentMutable);
            //
            Router.Navigate.Execute(new TestAVM(this));
        }

        public RoutingState Router { get; private set; }

        private void RegisterParts(IMutableDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterConstant(this, typeof(IScreen));
            //
            dependencyResolver.Register(() => new TestA(), typeof(IViewFor<TestAVM>));
            dependencyResolver.Register(() => new TestB(), typeof(IViewFor<TestBVM>));
        }

        public Page CreateMainPage()
        {
            return new RoutedViewHost();
        }
    }
}