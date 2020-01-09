using ReactiveUI;

namespace Shared.Configuration
{
    public class ProductionItemViewModel : ItemViewModel
    {
        public override string UrlPathSegment => "Production";
        public override int Id { get; } = 2;

        public override string Name { get; } = "Production Abc";

        public ProductionItemViewModel(IScreen screen = null) : base(screen)
        {

        }


        protected override void OnConfirmIfNew()
        {
            
        }
    }
}
