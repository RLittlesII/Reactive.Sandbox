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
        public ConfirmPopup()
        {
            InitializeComponent();

            var cancel = Cancel.Events().Pressed.Select(x => false);

            cancel
                .ObserveOn(RxApp.MainThreadScheduler)
                .InvokeCommand(this, x => x.ViewModel.Cancel)
                .DisposeWith(ViewBindings);

            var confirm = Confirm.Events().Pressed.Select(x => true);

            confirm
                .Merge(cancel)
                .StartWith(false)
                .DistinctUntilChanged()
                .Subscribe(result => Result = result)
                .DisposeWith(ViewBindings);

            confirm
                .Merge(cancel)
                .Subscribe(result =>
                {
                    Result = result;
                    PopupNavigation.Instance.PopAsync();
                })
                .DisposeWith(ViewBindings);
        }

        public bool Result { get; set; }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ControlBindings?.Dispose();
            ViewBindings?.Dispose();
        }
    }
}