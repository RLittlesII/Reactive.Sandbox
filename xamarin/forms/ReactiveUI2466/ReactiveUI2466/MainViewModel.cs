using ReactiveUI;
using Sextant;

namespace ReactiveUI2466
{
    public class MainViewModel : ReactiveObject, IViewModel
    {
        private bool _isReadOnly;
        public string Id { get; }

        public bool IsReadOnly
        {
            get => _isReadOnly;
            set => this.RaiseAndSetIfChanged(ref _isReadOnly, value);
        }
    }
}