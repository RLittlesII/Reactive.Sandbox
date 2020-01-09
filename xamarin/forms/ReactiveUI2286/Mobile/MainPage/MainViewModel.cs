using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Shared.Configuration;
using Splat;

namespace Mobile.Main
{
    public class MainViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "Main Page";
        public IScreen HostScreen { get; }

        public IEnumerable<MenuItemViewModel> MenuItems { get; }
        [Reactive] public MenuItemViewModel SelectedItem { get; set; }
        public ReactiveCommand<MenuItemViewModel, Unit> NavigateToMenuItem { get; }

        public MainViewModel(IScreen screen = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();

            MenuItems = GetMenuItems();

            NavigateToMenuItem = ReactiveCommand.CreateFromObservable<MenuItemViewModel, Unit>(
                x =>
                {
                    if (x != null)
                    {
                        HostScreen.Router.Navigate
                            .Execute(Locator.Current.GetService<IRoutableViewModel>(x.Type.FullName))
                            .Subscribe();
                    }

                    return Observable.Return(Unit.Default);
                });
        }

        private IEnumerable<MenuItemViewModel> GetMenuItems()
        {
            return new[]
            {
                new MenuItemViewModel("Check Out", "C", typeof(CheckOutItemViewModel))
            };
        }
    }
}
