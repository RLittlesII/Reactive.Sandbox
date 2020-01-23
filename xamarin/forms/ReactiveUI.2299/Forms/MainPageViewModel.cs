using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using Xamarin.Forms;

namespace Forms
{
    public class MainPageViewModel : ReactiveObject
    {
        int itemNumber = 1;
        readonly Random random;
        private bool _isRefreshing;

        public MainPageViewModel()
        {
            random = new Random();
            Items = new ObservableCollection<Item>();
            AddItems();

            XfRefreshCommand =
                new Command(() =>
                {
                    // When binding to this command, set the RefreshView.IsRefreshing binding to two-way in the view
                    RefreshItems()
                        .Select(_ => false)
                        .ObserveOn(RxApp.MainThreadScheduler)
                        .BindTo(this, x => x.IsRefreshing);
                });

            RxRefreshCommand = ReactiveCommand.CreateFromObservable<Unit, Unit>(
                _ => RefreshItems(),
                outputScheduler: RxApp.MainThreadScheduler);

            RxRefreshCommand
                .IsExecuting
                .Where(isExecuting => !isExecuting)
                .ObserveOn(RxApp.MainThreadScheduler)
                .BindTo(this, x => x.IsRefreshing);

            RxRefreshCommand
                .ThrownExceptions
                .Subscribe(ex => Debug.WriteLine(ex));

            RxRefreshCommand
                .Subscribe();
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => this.RaiseAndSetIfChanged(ref _isRefreshing, value);
        }

        public ObservableCollection<Item> Items { get; private set; }

        public ReactiveCommand<Unit, Unit> RxRefreshCommand { get; set; }

        public ICommand XfRefreshCommand { get; set; }

        void AddItems()
        {
            for (int i = 0; i < 50; i++)
            {
                Items.Add(new Item
                {
                    Color = Color.FromRgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255)),
                    Name = $"Item {itemNumber++}"
                });
            }
        }

        private IObservable<Unit> RefreshItems()
        {
            return Observable
                .Return(Unit.Default)
                .Delay(TimeSpan.FromSeconds(3), RxApp.TaskpoolScheduler)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Select(_ =>
                {
                    AddItems();
                    return Unit.Default;
                });
        }
    }
}
