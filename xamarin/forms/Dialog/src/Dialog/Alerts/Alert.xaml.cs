


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
        public Alert(AlertDetailModel alertDetailModel)
        {
            InitializeComponent();

            BindingContext = alertDetailModel;

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
    }
}