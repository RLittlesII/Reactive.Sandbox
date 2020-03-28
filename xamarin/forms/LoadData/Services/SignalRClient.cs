using System.Reactive.Subjects;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace Services
{
    public class SignalRClient : IHubClient
    {
        private HubConnection _connection;
        private ISubject<CoffeeBeanDto> _coffeeBeansSubject = new Subject<CoffeeBeanDto>();

        public SignalRClient(string connectionString)
        {
            _connection = new HubConnectionBuilder().WithUrl(connectionString).Build();
            _connection.On<CoffeeBeanDto>("GetCoffeeBeans", dto => _coffeeBeansSubject.OnNext(dto));
        }

        public async Task Connect() => await _connection.StartAsync().ConfigureAwait(false);

        public async Task InvokeAsync(string method) => await _connection.InvokeAsync(method).ConfigureAwait(false);

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                _connection.StopAsync();
                _connection?.DisposeAsync();
                _coffeeBeansSubject?.OnCompleted();
            }
        }
    }
}