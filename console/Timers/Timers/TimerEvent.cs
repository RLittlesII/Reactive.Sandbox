namespace Timers
{
    public class TimerEvent
    {
        public TimerEvent(TimerState state)
        {
            State = state;
        }

        public TimerState State { get; }
    }
}