using System;

namespace WhenAny
{
    public class WhenAnyExceptionHandler : IObserver<Exception>
    {
        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(Exception value) { }
    }
}