using System;
using System.Linq;
using System.Reactive.Disposables;
using CoreFoundation;
using UIKit;
using Foundation;
using ReactiveUI;
using System.Reactive.Linq;
using System.Windows.Input;
using CoreGraphics;
using DynamicData.Binding;

namespace ListView
{
    [Register("ListController")]
    public class ListController : ReactiveViewController<ListViewModel>
    {
        private static readonly CompositeDisposable ControlBindings = new CompositeDisposable();

        public TableView TableView { get; set; }

        public  UISegmentedControl ItemType { get; set; }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            CreateUserInterface();

            RegisterObservers();
        }

        public override void ViewWillAppear(bool animated)
        {
            this.ViewModel.SelectedItem = null;
            base.ViewWillAppear(animated);
        }

        private void CreateUserInterface()
        {
            base.ViewDidLoad();
            View = new UniversalView();

            ItemType = new UISegmentedControl
            {
                ContentMode = UIViewContentMode.ScaleToFill,
                Frame = new CGRect(16, 0, 343, 29),
                HorizontalAlignment = UIControlContentHorizontalAlignment.Left,
                VerticalAlignment = UIControlContentVerticalAlignment.Top,
                ControlStyle = UISegmentedControlStyle.Plain,
                SelectedSegment = 0,
                BackgroundColor = UIColor.FromRGBA(67, 96, 190, 180),
                TintColor = UIColor.Clear
            };

            ItemType.InsertSegment(ListView.ItemType.Some.ToString(), (nint)0, true);
            ItemType.InsertSegment(ListView.ItemType.Other.ToString(), (nint)1, true);
            ItemType.InsertSegment(ListView.ItemType.All.ToString(), (nint)2, true);
            ItemType.SetTitleTextAttributes(
                new UITextAttributes()
                {
                    Font = UIFont.FromName("Arial", 14),
                    TextColor = UIColor.FromRGBA(255, 255, 255, 180),
                }, UIControlState.Normal);

            ItemType.SetTitleTextAttributes(
                new UITextAttributes()
                {
                    Font = UIFont.FromName("Arial", 14),
                    TextColor = UIColor.FromRGBA(255, 255, 255, 255),
                }, UIControlState.Selected);

            SegmentedControlView = new UIView
            {
                Frame =  new CGRect(0, 112, 375, 50)
            };

            SegmentedControlView.AddSubview(ItemType);

            TableView = new TableView()
            {
                Frame = new CGRect(0, 150, 414, 500),
                DelaysContentTouches = false,
                SeparatorColor = UIColor.Black,
                TableFooterView = new UIView()
            };

            View.AddSubview(SegmentedControlView);
            View.AddSubview(TableView);

            TableView.RegisterClassForCellReuse(typeof(TableCell), TableCell.ReuseKey);
        }

        public UIView SegmentedControlView { get; set; }

        private void RegisterObservers()
        {
            ItemType
                .Events()
                .ValueChanged
                .Select(x => Convert.ToInt32(ItemType.SelectedSegment))
                .InvokeCommand(this, x => x.ViewModel.ChangeSegment)
                .DisposeWith(ControlBindings);

            var tableViewSource = this.WhenAnyValue(x => x.ViewModel.Items)
                .Select(x => x == null ? null : new TableSource(TableView, x, TableCell.ReuseKey))
                .Publish();

            tableViewSource
                .BindTo(TableView, x => x.Source)
                .DisposeWith(ControlBindings);

            tableViewSource
                .Where(x => x != null)
                .Select(x => x.ElementSelected)
                .Switch()
                .Cast<TableCellViewModel>()
                .Subscribe(x => ViewModel.SelectedItem = x)
                .DisposeWith(ControlBindings);

            tableViewSource.Connect().DisposeWith(ControlBindings);

            this.WhenAnyValue(x => x.ViewModel.LoadData)
                .WhereNotNull()
                .ToSignal()
                .InvokeCommand(this, x => x.ViewModel.LoadData)
                .DisposeWith(ControlBindings);

            this.Bind(ViewModel, x => x.SelectedSegment, x => x.ItemType.SelectedSegment)
                .DisposeWith(ControlBindings);
        }

    }
}