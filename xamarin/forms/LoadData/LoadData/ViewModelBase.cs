using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;

namespace LoadData
{
    public class ViewModelBase : ReactiveObject
    {
        public ViewModelBase()
        {
            InitializeData = ReactiveCommand.CreateFromTask(ExecuteInitializeData);
        }
        
        protected virtual Task ExecuteInitializeData() => Task.CompletedTask;
        
        public ReactiveCommand<Unit, Unit> InitializeData { get; }
    }
}