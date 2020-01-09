using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Main
{
    public class MenuItemViewModel : ReactiveObject
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public Type Type { get; set; }

        public MenuItemViewModel(string title, string icon, Type type)
        {
            Title = title;
            Icon = icon;
            Type = type;
        }
    }
}
