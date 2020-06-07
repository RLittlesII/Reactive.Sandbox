


using System;
using System.Reactive;
using System.Reactive.Linq;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Services;
using Rocket.Surgery.Airframe.Popup;
using Splat;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dialog.Alerts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Alert : PopupPageBase<AlertViewModel>, IEnableLogger
    {
        private readonly IPopupNavigation _navigation;

        public Alert(AlertDetail alertDetail)
        {
            InitializeComponent();

            BindingContext = alertDetail;

            Confirm
                .Events()
                .Pressed
                .Subscribe(_ => PopupNavigation.Instance.PopAsync());

            // Header.Source = viewModel.HeaderImage;
            // Title.Text = viewModel.Title;
            // Message.Text = viewModel.Message;
            // Completed = Observable
            //     .Timer(viewModel.Timeout, RxApp.TaskpoolScheduler)
            //     .TakeUntil(_cancelled)
            //     .ObserveOn(RxApp.MainThreadScheduler)
            //     .Select(_ => Pop())
            //     .Switch()
            //     .Publish()
            //     .RefCount();
        }

        public bool Result { get; set; }
    }
}