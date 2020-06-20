using System;

namespace Timers
{
    public interface ITimer
    {
        IObservable<TimerEvent> Changed { get; }
        void Start();
        void Stop();
    }
}