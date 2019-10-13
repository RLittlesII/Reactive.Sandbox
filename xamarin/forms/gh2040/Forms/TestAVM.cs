using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using ReactiveUI;
using Splat;

namespace Forms
{
    public class TestAVM : ReactiveObject, IEnableLogger, IRoutableViewModel
    {
        public TestAVM(IScreen hostScreen)
        {
            HostScreen = hostScreen;
            //
            TestBVM testBVM = new TestBVM(HostScreen);
            //
            ClickCommand = ReactiveCommand.CreateFromObservable(() =>
            {
                HostScreen.Router.Navigate.Execute(testBVM).Select(_ => Unit.Default).Subscribe();
                return Observable.Return(Unit.Default);
            });
            //
            testBVM
                .WhenNavigatingFromObservable()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe((_) => { ShowActivityIndicator = true; });
        }

        //
        public bool ShowActivityIndicator { get; set; }

        public ReactiveCommand<Unit, Unit> ClickCommand { get; set; }

        public IScreen HostScreen { get; }

        //
        public string UrlPathSegment => "TestA";
    }
}
