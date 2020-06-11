using System;
using Xamarin.Forms;

namespace Dialog
{
    public class ActionSheetDetailModel
    {
        public ActionSheetDetailModel(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; }

        public string Value { get; }
    }
}