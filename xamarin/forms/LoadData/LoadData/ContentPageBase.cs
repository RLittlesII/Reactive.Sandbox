using System.Reactive.Disposables;
using ReactiveUI.XamForms;

namespace LoadData
{
    public class ContentPageBase<T> : ReactiveContentPage<T>
        where T : class
    {
        protected readonly CompositeDisposable ViewBindings = new CompositeDisposable();
    }
}