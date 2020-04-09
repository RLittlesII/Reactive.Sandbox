using System;
using System.Reactive;
using System.Threading;

namespace CancellationToken
{
    public class ObservableCancellationTokenSource<T> : CancellationTokenSource
    {
        private readonly IDisposable _subscription;

        public ObservableCancellationTokenSource(IObservable<T> source)
        {
            _subscription = source.Subscribe(token => this.Cancel());
        }

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _subscription?.Dispose();
            }
        }
    }

    public static class ObservableCancellationTokenSourceExtensions
    {
        public static ObservableCancellationTokenSource<T> ToCancellationTokenSource<T>(this IObservable<T> source) => new ObservableCancellationTokenSource<T>(source);
    }
}