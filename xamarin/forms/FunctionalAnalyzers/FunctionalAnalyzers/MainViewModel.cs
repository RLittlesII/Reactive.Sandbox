using System;
using System.Reactive.Linq;
using ReactiveUI;
using Sextant;

namespace FunctionalAnalyzers
{
    public class MainViewModel : ReactiveObject, IViewModel
    {
        private readonly ISettingsProvider _settingsProvider;
        private readonly ObservableAsPropertyHelper<bool> _showAdmin;
        private bool _toggle;

        public MainViewModel(ISettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;

            // Good
            this.WhenAnyValue(x => x.Toggle,
                    x => x._settingsProvider.Settings.IsAdmin,
                    (toggle, admin) => toggle && admin)
                .ToProperty(this, nameof(ShowAdmin), out _showAdmin);

            // Not so good
            this.WhenAnyValue(x => x.Toggle)
                .Select(x => _settingsProvider.Settings.IsAdmin)
                .ToProperty(this, nameof(ShowAdmin), out _showAdmin);

            // Runtime Error
            this.WhenAnyValue(x => x.Toggle,
                    x => _settingsProvider.Settings.IsAdmin,
                    (toggle, admin) => toggle && admin)
                .ToProperty(this, nameof(ShowAdmin), out _showAdmin);
        }

        public string Id { get; }

        public bool Toggle
        {
            get => _toggle;
            set => this.RaiseAndSetIfChanged(ref _toggle, value);
        }

        public bool ShowAdmin => _showAdmin.Value;
    }
}