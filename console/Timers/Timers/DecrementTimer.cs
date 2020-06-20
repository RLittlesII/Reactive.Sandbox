using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ReactiveUI;

namespace Timers
{
    public class DecrementTimer : ReactiveObject
    {
        private readonly CompositeDisposable _timesUp = new CompositeDisposable();
        private readonly IScheduler _scheduler;
        private readonly Subject<bool> _running;
        private ObservableAsPropertyHelper<bool> _isRunning;
        private TimeSpan _pausedTime = TimeSpan.Zero;
        private TimeSpan _resumeTime;

        public DecrementTimer(IScheduler scheduler)
        {
            _scheduler = scheduler;
            _running = new Subject<bool>();
        }

        public bool IsRunning => _isRunning.Value;

        public IObservable<TimeSpan> Timer(TimeSpan startTime, bool startImmediately = true)
        {
            var refreshInterval = TimeSpan.FromMilliseconds(1000);
            var running = _running.AsObservable().StartWith(startImmediately);

            _isRunning = running.ToProperty(this, x => x.IsRunning).DisposeWith(_timesUp);
            _resumeTime = startTime;

            var timer =
                Observable
                    .Create<TimeSpan>(observer =>
                        Observable
                            .Interval(refreshInterval, _scheduler)
                            .Scan(_resumeTime, (acc, value) => acc - refreshInterval)
                            .TakeUntil(x => x <= TimeSpan.FromSeconds(-1))
                            .Do(x => _resumeTime = x, () => _resumeTime = startTime)
                            .Subscribe(observer));

            return
                running
                    .Select(isRunning => isRunning ? timer : Observable.Never<TimeSpan>())
                    .Switch()
                    .Publish()
                    .RefCount()
                    .ObserveOn(_scheduler);
        }

        public IObservable<bool> Start()
        {
            _running.OnNext(true);
            return Observable.Return(true);
        }

        public IObservable<bool> Pause()
        {
            _running.OnNext(false);
            return Observable.Return(false);
        }

        public IObservable<bool> Resume()
        {
            _running.OnNext(true);
            return Observable.Return(true);
        }
    }
}