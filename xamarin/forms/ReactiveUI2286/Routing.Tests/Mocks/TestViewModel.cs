using ReactiveUI;

namespace Routing.Tests
{
    public class TestViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment { get; }

        public IScreen HostScreen { get; }
    }
}