using ReactiveUI.XamForms;
using Xamarin.Forms.Xaml;

namespace DynamicList.Search
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchResultViewCell : ReactiveViewCell<SearchResultViewModel>
    {
        public SearchResultViewCell()
        {
            InitializeComponent();
        }
    }
}