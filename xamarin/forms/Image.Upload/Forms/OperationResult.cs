using Forms.Services;

namespace Forms
{
    public class OperationResult
    {
        public bool Success { get; set; }
        
        public OperationState State { get; set; }

        public IOperation Operation { get; set; }

        public OperationFailureException Exception { get; set; }
    }
}