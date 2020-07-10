using System;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using Sextant;
using Splash.Login;
using Splat;

namespace Splash.Splash
{
    public class SplashViewModel : ReactiveObject, IViewModel
    {
        public SplashViewModel()
        {
            Observable
                .Timer(TimeSpan.FromSeconds(3))
                .Select(x => Unit.Default)
                .Subscribe(x => Locator.Current.GetService<IParameterViewStackService>().PushPage<LoginViewModel>());
        }
        
        public string Id { get; }
    }
}