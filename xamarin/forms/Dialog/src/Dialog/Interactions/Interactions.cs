using System.Reactive;
using ReactiveUI;

namespace Dialog
{
    public static class Interactions
    {
        public static readonly Interaction<AlertDetail, Unit> ShowAlert = new Interaction<AlertDetail, Unit>();

        public static readonly Interaction<ActionSheetDetail, string> ShowActionSheet = new Interaction<ActionSheetDetail, string>(RxApp.MainThreadScheduler);

        public static readonly Interaction<ConfirmationDetail, bool> ShowConfirmation = new Interaction<ConfirmationDetail, bool>();
    }
}