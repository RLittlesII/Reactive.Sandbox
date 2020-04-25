using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using Splat;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Animations.Login
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IAccountsService _accountCredentialsService;
        private readonly IAccountService _accountService;
        private readonly IQLog _qLog;

        private Account _lastAccount;

        public LoginViewModel(IAccountService accountService = null,
            IScheduler mainThreadScheduler = null,
            IScheduler taskPoolScheduler = null,
            IScreen hostScreen = null,
            IQLog qLog = null)
            : base("Login View",
                mainThreadScheduler,
                taskPoolScheduler,
                hostScreen)
        {
            _accountService = accountService ?? Locator.Current.GetService<IAccountService>();
            _accountCredentialsService = _accountCredentialsService ?? Locator.Current.GetService<IAccountsService>();

            _qLog = qLog ?? Locator.Current.GetService<IQLog>();

            var versionService = DependencyService.Get<IAppVersionService>();
            AppVersion = versionService?.GetVersion() + " (" + versionService?.GetBuild().ToString() + ")";

            CompanyName = string.Empty;
            Username = new FormFieldViewModel<string>();
            Password = new FormFieldViewModel<string>();

            LoadCommand = ReactiveCommand
                .CreateFromTask<Unit, Unit>(async _ => { return Unit.Default; });

            LoginCommand = ReactiveCommand
                .CreateFromTask(Login);

            LoginCommand.IsExecuting
                //.Throttle(TimeSpan.FromMilliseconds(100))
                .ObserveOn(RxApp.MainThreadScheduler)
                .ToPropertyEx(this, x => x.IsBusy);

            LoadCommand
                .ThrownExceptions
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x => { _qLog.Log(x); });

            LoginCommand
                .ThrownExceptions
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x => { _qLog.Log(x); });

            AppVersionTappedCommand = ReactiveCommand.Create(() =>
            {
                // toggle sandbox visible state
                IsSandboxChoiceVisible = !IsSandboxChoiceVisible;
                // if we're hiding then ensure sandbox state is off
                if (!IsSandboxChoiceVisible)
                    IsSandbox = false;
            });
        }

        public ReactiveCommand<Unit, Unit> LoadCommand { get; }
        public ReactiveCommand<Unit, bool> LoginCommand { get; }
        public ReactiveCommand<Unit, Unit> AppVersionTappedCommand { get; set; }
        [Reactive] public bool IsSandbox { get; private set; }
        [Reactive] public bool IsSandboxChoiceVisible { get; set; }
        [Reactive] public string AppVersion { get; set; }
        [Reactive] public string CompanyName { get; }
        public FormFieldViewModel<string> Username { get; }
        public FormFieldViewModel<string> Password { get; }

        private async Task<bool> Login()
        {
            try
            {
                if (CompanyName.Trim().ToLower() == "debug" && IsSandbox)
                    HostScreen.Router.Navigate.Execute(new AkavacheExplorerViewModel()).Subscribe();

                if (!this.Validate().IsValid)
                    return false;

                var url = CompanyName.Trim().ToApiUri(Urls.prefix, IsSandbox ? Urls.quoodaSandbox : Urls.quoodaLive);

                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    var authResult = await _accountService
                        .Authenticate(Username.Value.Trim(), Password.Value.Trim(), url).ConfigureAwait(false);
                    if (authResult.State == AuthState.Authenticated)
                        HostScreen.Router.NavigateAndReset.Execute(new SmartFormOverviewViewModel()).Subscribe();
                    else
                        Password.ErrorMessage = "Could not authenticate. Please check your details";
                }
                else
                {
                    if (_lastAccount != null)
                    {
                        bool passwordValid = _accountService.VerifyPassword(Password.Value.Trim(),
                            _lastAccount.hashSalt.Hash, _lastAccount.hashSalt.Salt);
                        // TODO no network connection - perform 'offline' login comparing pwd salt with cached account details
                        // this can only be done if the credentials match the last account details otherwise an offline
                        // login to a new server / user would be done, which would cause problems within the app
                        if (url == _lastAccount.Domain && Username.Value.Trim() == _lastAccount.Username &&
                            passwordValid)
                            // typed information matches stored last credentials
                            HostScreen.Router.NavigateAndReset.Execute(new SmartFormOverviewViewModel()).Subscribe();
                        else
                            Password.ErrorMessage = "Could not authenticate. Please check your details";
                    }
                }
            }
            catch (Exception ex)
            {
                Username.ErrorMessage = "Something went wrong, please try again";
                _qLog.Log(ex);
            }

            return true;
        }

        public override void BuildValidationRules()
        {
            //this
            //    .GetValidator()
            //    .RuleFor(vm => vm.CompanyName.Value)
            //    .Must(x => IsValidCompany(x))
            //    .WithMessage("Oops, we couldn't find your company?")
            //    .ToFieldError(CompanyName);

            this
                .GetValidator()
                .RuleFor(vm => vm.Username.Value)
                .NotNull()
                .NotEmpty()
                .WithMessage(LoginResx.UsernameErrorMessage)
                .ToFieldError(Username);

            this
                .GetValidator()
                .RuleFor(vm => vm.Password.Value)
                .NotNull()
                .NotEmpty()
                .WithMessage(LoginResx.PasswordErrorMessage)
                .ToFieldError(Password);
        }

        private bool IsValidCompany(string companyName)
        {
            return string.IsNullOrEmpty(companyName);
        }
    }

    public static class StringHelpers
    {
        public static string ToApiUri(this string companyName, string prefix, string domain)
        {
            return $"{prefix}{companyName}{domain}";
        }
    }
}