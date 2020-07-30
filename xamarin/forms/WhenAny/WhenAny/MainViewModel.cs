using ReactiveUI;
using Sextant;

namespace WhenAny
{
    public class MainViewModel : ReactiveObject, IViewModel
    {
        public MainViewModel()
        {
            this.WhenAny()
            
        }

        public string Id { get; }
    }
}