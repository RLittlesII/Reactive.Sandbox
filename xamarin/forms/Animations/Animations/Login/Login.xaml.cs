using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Animations.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : BaseContentPageRxUI<LoginViewModel>
    {
        public Login()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                Observable.Where(this
                        .WhenAnyValue(v => v.ViewModel.LoadCommand), x => !Loaded)
                    .Do(x => Loaded = true)
                    .Select(x => Unit.Default)
                    .InvokeCommand(ViewModel.LoadCommand)
                    .DisposeWith(disposables);

                this
                    .BindCommand(ViewModel,
                        vm => vm.LoginCommand,
                        v => v.NextButton)
                    .DisposeWith(disposables);

                this
                    .Bind(ViewModel,
                        vm => vm.IsSandbox,
                        v => v.SandboxCheckbox.IsChecked)
                    .DisposeWith(disposables);

                this
                    .Bind(ViewModel,
                        vm => vm.IsSandboxChoiceVisible,
                        v => v.SandboxChoiceControls.IsVisible)
                    .DisposeWith(disposables);

                this
                    .Bind(ViewModel,
                        vm => vm.AppVersion,
                        v => v.AppVersion.Text)
                    .DisposeWith(disposables);

                this
                    .Bind(ViewModel,
                        vm => vm.CompanyName,
                        v => v.CompanyName.Text)
                    .DisposeWith(disposables);

                this
                    .BindCommand(ViewModel,
                        vm => vm.AppVersionTappedCommand,
                        v => v.AppVersionTapped)
                    .DisposeWith(disposables);

                this
                    .WhenAnyValue(x => x.ViewModel.IsBusy)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(busy =>
                    {
                        if (busy)
                        {
                            Username.Unfocus();
                            Password.Unfocus();
                        }

                        NextButton.IsLoading = busy;
                        NextButton.IsEnabled = !busy;
                    })
                    .DisposeWith(disposables);

                this
                    .WhenAnyValue(x => x.ViewModel.IsSandbox)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(async isSandbox =>
                    {
                        SandboxCheckbox.Unfocus();
                        SandboxCheckbox.IsEnabled = false;
                        await Domain.FadeTo(0, 200, Easing.SpringOut);
                        if (isSandbox)
                            Domain.Text = LoginResx.DomainSandbox;
                        else
                            Domain.Text = LoginResx.DomainLive;
                        await Domain.FadeTo(1, 200, Easing.SpringIn);
                        SandboxCheckbox.IsEnabled = true;
                    }).DisposeWith(disposables);
            });
        }

        private async void CompanyName_Focused(object sender, FocusEventArgs e)
        {
            await ScrollViewContainer.ScrollToAsync(CompanyName, ScrollToPosition.Start, true);
        }

        private void CompanyNameFrame_Tapped(object sender, EventArgs e)
        {
            CompanyName.Focus();
        }
    }
}