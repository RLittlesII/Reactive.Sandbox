using System;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using DynamicData.Alias;
using ReactiveUI;
using ReactiveUI.XamForms;
using Rg.Plugins.Popup.Services;
using Splat;
using Xamarin.Forms;

namespace DynamicList.Crud
{
    public partial class List : ReactiveContentPage<ListViewModel>
    {
        private readonly CompositeDisposable ViewBindings = new CompositeDisposable();

        public List()
        {
            InitializeComponent();

            this.OneWayBind(ViewModel, x => x.IsRefreshing, x => x.ListView.IsRefreshing)
                .DisposeWith(ViewBindings);

            this.Bind(ViewModel, x => x.SearchText, x => x.Search.Text)
                .DisposeWith(ViewBindings);

            ListView
                .Events()
                .Refreshing
                .InvokeCommand(this, x => x.ViewModel.Refresh)
                .DisposeWith(ViewBindings);

            ListView
                .Events()
                .ItemSelected
                .Subscribe(item =>
                {
                    ListView.SelectedItem = null;
                })
                .DisposeWith(ViewBindings);

            Add.Events()
                .Pressed
                .InvokeCommand(this, x => x.ViewModel.Add)
                .DisposeWith(ViewBindings);

            this.WhenAnyValue(x => x.ViewModel.Items)
                .Where(x => x != null)
                .BindTo(this, x => x.ListView.ItemsSource)
                .DisposeWith(ViewBindings);

            Interactions
                .AddItem
                .RegisterHandler(context =>
                {
                    // HACK: [rlittlesii: July 03, 2020] This is why "service location is an anti-pattern".
                    // HACK: [rlittlesii: July 03, 2020] Because it allows developers to implement bad patterns.
                    // HACK: [rlittlesii: July 03, 2020] Service Location is a tool that can be abused, not a pattern!
                    NewItem confirmationPage = (NewItem)Locator.Current.GetService<IViewFor<NewItemViewModel>>();

                    PopupNavigation
                        .Instance
                        .PushAsync(confirmationPage)
                        .ToObservable()
                        .ObserveOn(RxApp.MainThreadScheduler)
                        .ForkJoin(
                            confirmationPage
                                .Events()
                                .Disappearing
                                .Take(1)
                                .Select(x => Unit.Default),
                            (_, __) => __)
                        .Subscribe(result =>
                        {
                            context.SetOutput(Unit.Default);
                        });

                })
                .DisposeWith(ViewBindings);
        }
    }
}
