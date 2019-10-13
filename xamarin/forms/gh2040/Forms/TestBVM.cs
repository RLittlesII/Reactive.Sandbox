using System.Reactive;
using ReactiveUI;
using Splat;

namespace Forms
{
    public class TestBVM : ReactiveObject, IEnableLogger, IRoutableViewModel
    {
        public TestBVM(IScreen hostScreen)
        {
            _HostScreen = hostScreen;
        }
        //
        public ReactiveCommand<Unit, Unit> ClickCommand => _HostScreen.Router.NavigateBack;
        //
        private readonly IScreen _HostScreen; public IScreen HostScreen => _HostScreen;
        //
        public string UrlPathSegment => "TestB";
    }
}