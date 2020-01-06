using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Forms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class One : ContentPageBase<OneViewModel>
    {
        public One()
        {
            InitializeComponent();

            this.BindCommand(ViewModel, x => x.Navigate, x => x.Navigate)
                .DisposeWith(ViewBindings);
        }
    }
}