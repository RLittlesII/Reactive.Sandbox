using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Forms.Services
{
    public interface IOperation
    {
        IObservable<OperationResult> Execute(); // Function that executes all upload operations for a give image.
    }

    public class Operation : IOperation
    {
        public virtual IObservable<OperationResult> Execute() => Observable.Return(default(OperationResult));
    }
}