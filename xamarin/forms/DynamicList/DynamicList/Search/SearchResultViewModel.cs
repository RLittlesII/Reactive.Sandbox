using DynamicList.Crud;
using ReactiveUI;

namespace DynamicList.Search
{
    public class SearchResultViewModel : ItemViewModel
    {
        private string _deepLink;
        private string _url;

        public string DeepLink
        {
            get => _deepLink;
            set => this.RaiseAndSetIfChanged(ref _deepLink, value);
        }

        public string Url
        {
            get => _url;
            set => this.RaiseAndSetIfChanged(ref _url, value);
        }
    }
}