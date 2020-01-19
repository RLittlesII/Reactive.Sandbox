namespace Routing.Tests
{
    public class OneTestViewModel : ItemViewModel
    {
        public override string UrlPathSegment { get; }
        public override int Id { get; }
        public override string Name { get; }

        protected override void OnConfirmIfNew()
        {
            NextViewModel = new TwoTestViewModel();
        }
    }
}