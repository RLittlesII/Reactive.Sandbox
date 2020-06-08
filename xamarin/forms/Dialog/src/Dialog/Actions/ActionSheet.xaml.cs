using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.Surgery.Airframe.Popup;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dialog.Actions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActionSheet : PopupPageBase<ActionSheetViewModel>
    {
        public ActionSheet(ActionDetailModel detail)
        {
            InitializeComponent();
            BindingContext = detail;
        }

        public string Result { get; set; }
    }
}