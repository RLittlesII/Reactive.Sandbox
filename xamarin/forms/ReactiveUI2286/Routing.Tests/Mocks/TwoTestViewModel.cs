namespace Routing.Tests
{
    public class TwoTestViewModel : ItemViewModel
    {
        public override string UrlPathSegment { get; }
        public override int Id { get; }
        public override string Name { get; }

        protected override void OnConfirmIfNew()
        {
            throw new System.NotImplementedException();
        }
    }
}