using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using Rocket.Surgery.Airframe.Popup;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dialog.Actions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActionSheet : PopupPageBase<ActionSheetViewModel>
    {
        public ActionSheet(ActionSheetModel detail)
        {
            InitializeComponent();
            BindingContext = detail;

            this.WhenAnyValue(x => x.BindingContext)
                .Cast<ActionSheetModel>()
                .Where(x => x.Buttons != null)
                .Select(x => x.Buttons)
                .BindTo(this, x => x.Sheet.ItemsSource);
        }

        public TapGestureRecognizer Tapped { get; }
        public string Result { get; set; }
    }
}