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

            TestBVM testBVM = new TestBVM(HostScreen);

            testBVM
                .WhenNavigatingFromObservable()
                .Subscribe(_ => { ShowActivityIndicator = true; });

            ClickCommand = ReactiveCommand.CreateFromObservable(() => HostScreen.Router.Navigate.Execute(testBVM).Select(_ => Unit.Default));
        }

        //
        public bool ShowActivityIndicator { get; set; }

        public ReactiveCommand<Unit, Unit> ClickCommand { get; set; }

        public IScreen HostScreen { get; }

        //
        public string UrlPathSegment => "TestA";
    }
}
