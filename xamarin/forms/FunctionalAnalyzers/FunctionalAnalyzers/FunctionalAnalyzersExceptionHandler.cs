using System;

namespace FunctionalAnalyzers
{
    public class FunctionalAnalyzersExceptionHandler : IObserver<Exception>
    {
        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(Exception value) { }
    }
}