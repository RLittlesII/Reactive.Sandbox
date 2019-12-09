using System;
using System.Reactive.Linq;

namespace Forms.Services
{
    public abstract class Operation : IOperation
    {
        public virtual IObservable<OperationResult> Execute() => Observable.Return(default(OperationResult));
    }
}