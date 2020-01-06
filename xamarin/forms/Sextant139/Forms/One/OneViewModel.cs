using System;
using System.Reactive;
using ReactiveUI;
using Sextant;
using Splat;

namespace Forms
{
    public class OneViewModel : ViewModelBase
    {
        public OneViewModel()
        {
            Navigate = ReactiveCommand.CreateFromObservable(
                () => Locator.Current.GetService<IParameterViewStackService>().PushPage(new TwoViewModel()),
                outputScheduler: RxApp.MainThreadScheduler);
        }

        public ReactiveCommand<Unit, Unit> Navigate { get; set; }
    }
}