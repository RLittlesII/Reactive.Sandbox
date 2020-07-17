using System;
using System.Reactive.Linq;
using ReactiveUI;

namespace FunctionalAnalyzers
{
    public interface ISettingsProvider
    {
        ApplicationSettings Settings { get; }
    }

    public class SettingsProvider : ISettingsProvider
    {
        public SettingsProvider()
        {
            Observable.Interval(TimeSpan.FromSeconds(3))
                .Subscribe(x =>
                {
                    Settings.Environment = Guid.NewGuid().ToString();
                    Settings.IsAdmin = !Settings.IsAdmin;
                });
        }
        public ApplicationSettings Settings { get; } = new ApplicationSettings();
    }

    public class ApplicationSettings : ReactiveObject
    {
        private bool _isAdmin;
        private string _environment;

        public string Environment
        {
            get => _environment;
            set => this.RaiseAndSetIfChanged(ref _environment, value);
        }

        public bool IsAdmin
        {
            get => _isAdmin;
            set => this.RaiseAndSetIfChanged(ref _isAdmin, value);
        }
    }
}