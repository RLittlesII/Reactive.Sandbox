


using System;
using System.Reactive;
using System.Reactive.Linq;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Services;
using Rocket.Surgery.Airframe.Popup;
using Splat;
using Xamarin.Forms.Xaml;

namespace Dialog.Alerts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Alert : PopupPageBase<AlertViewModel>, IEnableLogger
    {
        private readonly IPopupNavigation _navigation;

        public Alert()
        {
            InitializeComponent();

            _navigation = PopupNavigation.Instance;
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

        public IObservable<Unit> Completed => BackgroundClick;

        protected override bool OnBackgroundClicked()
        {
            return base.OnBackgroundClicked();
        }
        protected override void OnDisappearing()
        {
            TryCleanup();
            base.OnDisappearing();
        }
        private void TryCleanup()
        {
            try
            {
            }
            catch (ObjectDisposedException)
            {
                this.Log().Info("Failed on disposing the cancellation subject in the PopupTimeoutView, the object is already disposed");
            }
        }
        private IObservable<Unit> Pop()
            => Observable.FromAsync(_ => _navigation.PopAsync());
    }
}