using ReactiveUI;

namespace Shared.Configuration
{
    public class CheckOutItemViewModel : ItemViewModel
    {
        public override string UrlPathSegment => "CheckOut";

        public override int Id { get; } = 1;

        public override string Name { get; } = "Check Out";

        public CheckOutItemViewModel(IScreen screen = null) : base(screen)
        {

        }

        protected override void OnConfirmIfNew()
        {
            // Open production item.
            //DisableNavigateBackOnCompleted = true;
            //HostScreen.Router.NavigationStack.RemoveAt(HostScreen.Router.NavigationStack.Count - 1);
            //HostScreen.Router.Navigate.Execute(GetProductionItemViewModelFromFields()).Subscribe();

            NextViewModel = GetProductionItemViewModelFromFields();
        }

        private ProductionItemViewModel GetProductionItemViewModelFromFields()
        {
            return new ProductionItemViewModel(HostScreen);
        }
    }
}
