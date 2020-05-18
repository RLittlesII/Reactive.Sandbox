using ReactiveUI;

namespace Dialog
{
    public static class Interactions
    {
        public static readonly Interaction<AlertDetail, bool> ShowAlert = new Interaction<AlertDetail, bool>();

        public static readonly Interaction<ActionSheetDetail, string> ShowActionSheet = new Interaction<ActionSheetDetail, string>(RxApp.MainThreadScheduler);
    }
}