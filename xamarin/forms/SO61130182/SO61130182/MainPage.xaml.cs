using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.XamForms;
using Xamarin.Forms;

namespace SO61130182
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ReactiveContentPage<MainViewModel>
    {
        public MainPage()
        {
            InitializeComponent();
            ViewModel = new MainViewModel();

            this.BindCommand(ViewModel, x => x.CommandOne, x => x.ThingOne);
            this.BindCommand(ViewModel, x => x.CommandTwo, x => x.ThingTwo);
        }
    }

    public class MainViewModel : ReactiveObject
    {
        public MainViewModel()
        {
            var commandOneCanExecute = this.WhenAnyObservable(x => x.CommandTwo.IsExecuting).StartWith(false).Select(x => !x);
            var commandTwoCanExecute = this.WhenAnyObservable(x => x.CommandOne.IsExecuting).StartWith(false).Select(x => !x);
            CommandOne = ReactiveCommand.CreateFromObservable(ExecuteCommand, commandOneCanExecute);
            CommandTwo = ReactiveCommand.CreateFromObservable(ExecuteCommand, commandTwoCanExecute);
        }

        public ReactiveCommand<Unit, Unit> CommandOne { get; set; }

        public ReactiveCommand<Unit, Unit> CommandTwo { get; set; }

        private IObservable<Unit> ExecuteCommand() => Observable.Return(Unit.Default).Delay(TimeSpan.FromSeconds(5));
    }
}
