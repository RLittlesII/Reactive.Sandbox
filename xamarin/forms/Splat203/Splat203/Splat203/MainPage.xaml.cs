using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.XamForms;
using Splat;
using Xamarin.Forms;

namespace Splat203
{
    public partial class MainPage : ReactiveContentPage<MainViewModel>
    {
        public MainPage()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                this.BindCommand(ViewModel, vm => vm.Clicked, page => page.Button);
            });
        }
    }

    public class MainViewModel : ReactiveObject, IRoutableViewModel
    {
        public MainViewModel() { Clicked = ReactiveCommand.CreateFromTask(async () => await ExecuteClicked()); }

        public ReactiveCommand<Unit, Unit> Clicked { get; set; }

        private async Task ExecuteClicked()
        {
            var bitmap = await BitmapLoader.Current.LoadFromResource("res:img", 300, 300);

            bitmap.Dispose();
        }

        public string UrlPathSegment { get; }

        public IScreen HostScreen => Locator.Current.GetService<IScreen>();
    }
}
