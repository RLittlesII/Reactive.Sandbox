using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Windows.Input;
using ReactiveUI;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Rocket.Surgery.Airframe.Popup;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dialog
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfirmPopup : PopupPageBase<ConfirmViewModel>
    {
        private IDisposable _cancelCommand;

        public ConfirmPopup()
        {
            InitializeComponent();

            this.OneWayBind(ViewModel, x => x.Message, x => x.Message.Text)
                .DisposeWith(ViewBindings);

            var cancel = Cancel
                .Events()
                .Pressed;

            cancel
                .ObserveOn(RxApp.MainThreadScheduler)
                .InvokeCommand(this, x => x.ViewModel.Cancel);

            var confirm =
                Confirm
                    .Events()
                    .Pressed;

            confirm
                .Select(x => true)
                .Merge(cancel.Select(x => false))
                .StartWith(false)
                .DistinctUntilChanged()
                .Subscribe(_ => Result = _);

            confirm
                .Merge(cancel)
                .Subscribe(_ => PopupNavigation.Instance.PopAsync());
        }

        public bool Result { get; set; }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ViewBindings?.Dispose();
        }
    }
}