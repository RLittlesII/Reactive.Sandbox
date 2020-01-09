using System;
using System.Reactive.Linq;
using Mobile.Configuration;
using Mobile.Main;
using ReactiveUI;
using ReactiveUI.XamForms;
using Splat;
using Xamarin.Forms;
using DynamicData;
using Shared.Configuration;

namespace Mobile
{
    /// <summary>
    /// The app bootstrapper which is used to register everything with the Splat service locator.
    /// It is also the central location for the RoutingState used for routing between views.
    /// </summary>
    public class AppBootstrapper : ReactiveObject, IScreen
    {
        /// <summary>
        /// Gets or sets the router which is used to navigate between views.
        /// </summary>
        public RoutingState Router { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppBootstrapper"/> class.
        /// </summary>
        public AppBootstrapper()
        {
            // Create a new Router.
            Router = new RoutingState();

            // Register this class instance as IScreen.
            Locator.CurrentMutable.RegisterConstant(this, typeof(IScreen));

            RegisterViews();
            RegisterViewModels();

            // Load main page.
            Router.NavigateAndReset.Execute(new MainViewModel()).Subscribe();

            Router
                .NavigationChanged
                .Where(x => x != null)
                .DistinctUntilChanged()
                .ForEachChange(x =>
                {
                    if (x.Item.Reason == ListChangeReason.Add)
                    {
                        Console.WriteLine(
                            $"ADD: {x.Item.Current?.UrlPathSegment} COUNT: {Router.NavigationStack.Count} TIME: {DateTime.Now}\n\n\n");
                    }
                    else if (x.Item.Reason == ListChangeReason.Remove)
                    {
                        Console.WriteLine(
                            $"REMOVE: {x.Item.Current?.UrlPathSegment} COUNT: {Router.NavigationStack.Count} TIME: {DateTime.Now}\n\n\n");
                    }
                }).Subscribe();
        }

        private void RegisterViews()
        {
            // Other pages.
            Locator.CurrentMutable.Register(() => new MainPage(), typeof(IViewFor<MainViewModel>));
            Locator.CurrentMutable.Register(() => new ItemPage(), typeof(IViewFor<CheckOutItemViewModel>));
            Locator.CurrentMutable.Register(() => new ItemPage(), typeof(IViewFor<ProductionItemViewModel>));
        }

        private void RegisterViewModels()
        {
            // Here, we use contracts to distinguish which routable view model we want to instantiate.
            // This helps us avoid a manual cast to IRoutableViewModel when calling Router.Navigate.Execute(...)
            Locator.CurrentMutable.Register(() => new CheckOutItemViewModel(), typeof(IRoutableViewModel), typeof(CheckOutItemViewModel).FullName);
            Locator.CurrentMutable.Register(() => new ProductionItemViewModel(), typeof(IRoutableViewModel), typeof(ProductionItemViewModel).FullName);
        }

        /// <summary>
        /// Creates the first main page used within the application.
        /// </summary>
        /// <returns>The page generated.</returns>
        public static Page CreateMainPage()
        {
            var a = new RoutedViewHost();
            Observable.FromEventPattern(a, nameof(a.Popped)).Subscribe(x =>
            {
                var args = x.EventArgs as NavigationEventArgs;
                Console.WriteLine($"\n\n\nPOP: {args.Page.Title} COUNT: {a.Navigation.NavigationStack.Count} TIME: {DateTime.Now}\n\n\n");
            });

            Observable.FromEventPattern(a, nameof(a.Pushed)).Subscribe(x =>
            {
                var args = x.EventArgs as NavigationEventArgs;
                Console.WriteLine($"\n\n\nPUSH: {args.Page.Title} COUNT: {a.Navigation.NavigationStack.Count} TIME: {DateTime.Now}\n\n\n");
            });


            // NB: This returns the opening page that the platform-specific
            // boilerplate code will look for. It will know to find us because
            // we've registered our AppBootstrappScreen.
            return a;
        }
    }
}
