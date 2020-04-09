using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using Xamarin.Essentials;

namespace CancellationToken
{
    public class MainViewModel : ReactiveObject
    {
        private ObservableCancellationTokenSource<ConnectivityChangedEventArgs> _cancellationTokenSource;

        public MainViewModel()
        {

            var connectivityChanges =
                Observable
                    .FromEvent<EventHandler<ConnectivityChangedEventArgs>, ConnectivityChangedEventArgs>(eventHandler =>
                        {
                            void Handler(object sender, ConnectivityChangedEventArgs eventArgs) =>
                                eventHandler(eventArgs);
                            return Handler;
                        },
                        x => Connectivity.ConnectivityChanged += x,
                        x => Connectivity.ConnectivityChanged -= x);

            _cancellationTokenSource =
                connectivityChanges
                    .FirstAsync(x =>
                        x.NetworkAccess != NetworkAccess.Internet ||
                        Connectivity
                            .ConnectionProfiles
                            .All(connectionProfile => connectionProfile != ConnectionProfile.WiFi))
                    .ToCancellationTokenSource();

            GetData = ReactiveCommand.CreateFromTask(ExecuteGetData);
        }

        public ReactiveCommand<Unit, Unit> GetData { get; set; }

        private Task ExecuteGetData()
        {
            using (_cancellationTokenSource)
            {
                
            }
        }
    }
}