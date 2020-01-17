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
            ClickCommand = _HostScreen.Router.NavigateBack;
        }
        //
        public ReactiveCommand<Unit, Unit> ClickCommand { get; set; }
        //
        private readonly IScreen _HostScreen; public IScreen HostScreen => _HostScreen;
        //
        public string UrlPathSegment => "TestB";
    }
}