using System.Reactive.Disposables;
using ReactiveUI;

namespace DynamicList
{
    public class ItemViewModelBase : ReactiveObject
    {
        protected readonly CompositeDisposable ItemSubscriptions = new CompositeDisposable();
    }
}