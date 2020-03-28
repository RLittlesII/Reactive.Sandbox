using System.Threading.Tasks;

namespace Services
{
    public class HubClientMock : IHubClient
    {
        public Task Connect() => Task.CompletedTask;

        public virtual Task InvokeAsync(string method) => Task.CompletedTask;
    }
}