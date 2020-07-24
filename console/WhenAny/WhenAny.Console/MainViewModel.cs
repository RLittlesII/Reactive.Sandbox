using System;
using System.Reactive.Linq;
using ReactiveUI;

namespace WhenAny.Console
{
    public class MainViewModel : ReactiveObject
    {
        private DateTimeOffset _time;
        private DateTimeOffset _timeTwo;
        private ObservableAsPropertyHelper<string> _timeSync;

        public MainViewModel()
        {
            this.WhenAnyValue(
                    x => x.Time, 
                    x => x.TimeTwo,
                    (time, timeTwo) => $"{time}: {timeTwo}")
                .ObserveOn(RxApp.MainThreadScheduler)
                .ToProperty(this, nameof(TimeSync), out _timeSync, scheduler: RxApp.TaskpoolScheduler);

            Observable
                .Interval(TimeSpan.FromSeconds(1))
                .Select(_ => DateTimeOffset.Now)
                .Subscribe(time =>
                {
                    Time = time;
                });

            Observable
                .Interval(TimeSpan.FromSeconds(2))
                .Select(_ => DateTimeOffset.Now)
                .Subscribe(time =>
                {
                    TimeTwo = time;
                });
        }

        public string TimeSync => _timeSync.Value;

        public DateTimeOffset Time
        {
            get => _time;
            set => this.RaiseAndSetIfChanged(ref _time, value);
        }

        public DateTimeOffset TimeTwo
        {
            get => _timeTwo;
            set => this.RaiseAndSetIfChanged(ref _timeTwo, value);
        }
    }
}