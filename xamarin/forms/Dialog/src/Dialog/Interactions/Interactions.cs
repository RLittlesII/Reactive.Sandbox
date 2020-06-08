using System.Reactive;
using ReactiveUI;

namespace Dialog
{
    public static class Interactions
    {
        public static readonly Interaction<AlertDetailModel, Unit> ShowAlert = new Interaction<AlertDetailModel, Unit>();

        public static readonly Interaction<ActionDetailModel, string> ShowActionSheet = new Interaction<ActionDetailModel, string>(RxApp.MainThreadScheduler);

        public static readonly Interaction<ConfirmDetailModel, bool> ShowConfirmation = new Interaction<ConfirmDetailModel, bool>();
    }
}