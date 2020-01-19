using ReactiveUI;

namespace Routing.Tests
{
    public class Screen : IScreen
    {
        public Screen()
        {
            Router = new RoutingState();
        }

        public RoutingState Router { get; }
    }
}