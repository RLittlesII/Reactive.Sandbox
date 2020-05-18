namespace Dialog
{
    public class ActionSheetDetail
    {
        public ActionSheetDetail(string title, string cancel, string destruction, params string[] buttons)
        {
            Title = title;
            Cancel = cancel;
            Destruction = destruction;
            Buttons = buttons;
        }

        public string Title { get; }

        public string Cancel { get; }

        public string Destruction { get; }

        public string[] Buttons { get; }
    }
}