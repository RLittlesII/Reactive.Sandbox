using System;
using System.Threading.Tasks;

namespace Forms.Services
{
    public interface IOperation
    {
        IObservable<OperationResult> Execute(); // Function that executes all upload operations for a give image.
    }
}