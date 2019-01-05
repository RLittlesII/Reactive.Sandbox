using System;
using ReactiveUI;
using ReactiveUI.XamForms;
using Splat;
using Xamarin.Forms;

namespace GH1873
{
    internal class Screen : ReactiveObject, IScreen
    {
        public RoutingState Router { get; }

        public Screen()
        {
            Router = new RoutingState();

            Locator.CurrentMutable.RegisterConstant(this, typeof(IScreen));

            Router
                .NavigateAndReset
                .Execute(new FirstViewModel())
                .Subscribe();
        }

        public Page PresentDefaultView() => new RoutedViewHost();
    }
}