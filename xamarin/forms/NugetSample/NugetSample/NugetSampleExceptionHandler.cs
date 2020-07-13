using System;
using Splat;

namespace NugetSample
{
    public class NugetSampleExceptionHandler : IObserver<Exception>, IEnableLogger
    {
        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(Exception value)
        {
            this.Log().Error(value);
        }
    }
}