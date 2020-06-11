using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dialog.Actions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActionSheetDetail : ContentView
    {
        public ActionSheetDetail()
        {
            InitializeComponent();

            Tapped =
                Observable
                    .FromEvent<EventHandler, EventArgs>(
                        eventHandler =>
                        {
                            void Handler(object sender, EventArgs args) => eventHandler(args);
                            return Handler;
                        },
                        x => TapGesture.Tapped += x,
                        x => TapGesture.Tapped -= x)
                    .Select(_ => new TappedEventArgs(this));
        }

        public TapGestureRecognizer TapGesture { get; }

        public IObservable<TappedEventArgs> Tapped { get; }
    }
}