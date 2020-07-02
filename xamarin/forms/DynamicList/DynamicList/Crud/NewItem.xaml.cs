using System;
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

namespace DynamicList.Crud
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewItem : PopupPageBase<NewItemViewModel>
    {
        public NewItem(NewItemViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;

            Save
                .Events()
                .Pressed
                .Select(x => Unit.Default)
                .InvokeCommand(this, x => x.ViewModel.Save);

            Cancel
                .Events()
                .Pressed
                .Select(x => Unit.Default)
                .InvokeCommand(this, x => x.ViewModel.Cancel);
        }
    }
}