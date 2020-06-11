using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Dialog
{
    public class ActionSheetModel
    {
        public ActionSheetModel(string title, string cancel, string destruction, params string[] buttons)
        {
            Title = title;
            Cancel = cancel;
            Destruction = destruction;
            Buttons = buttons.Select(x => new ActionSheetDetailModel(x, x));
        }

        public string Title { get; }

        public string Cancel { get; }

        public string Destruction { get; }

        public IEnumerable<ActionSheetDetailModel> Buttons { get; }
    }
}