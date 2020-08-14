using System;

namespace ReactiveUI2466
{
    public class ReactiveUI2466ExceptionHandler : IObserver<Exception>
    {
        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(Exception value) { }
    }
}