using System.Collections;
using System.Collections.Generic;

namespace Dialog
{
    public class ActionDetailModel
    {
        public ActionDetailModel(string title, string cancel, string destruction, params string[] buttons)
        {
            Title = title;
            Cancel = cancel;
            Destruction = destruction;
            Buttons = buttons;
        }

        public string Title { get; }

        public string Cancel { get; }

        public string Destruction { get; }

        public IEnumerable<string> Buttons { get; }
    }
}