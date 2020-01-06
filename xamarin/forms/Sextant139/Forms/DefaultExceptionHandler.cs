using System;

namespace Forms
{
    public class DefaultExceptionHandler : IObserver<Exception>
    {
        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(Exception value)
        {
        }
    }
}