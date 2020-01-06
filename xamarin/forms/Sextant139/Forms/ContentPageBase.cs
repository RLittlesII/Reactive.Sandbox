using System.Reactive.Disposables;
using ReactiveUI.XamForms;

namespace Forms
{
    public abstract class ContentPageBase<TViewModel> : ReactiveContentPage<TViewModel>
        where TViewModel : ViewModelBase
    {
        protected CompositeDisposable ViewBindings { get; } = new CompositeDisposable();
    }
}