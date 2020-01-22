using System;
using System.Linq;
using System.Reactive.Disposables;
using CoreFoundation;
using UIKit;
using Foundation;
using ReactiveUI;
using System.Reactive.Linq;
using System.Windows.Input;
using DynamicData.Binding;

namespace ListView
{
    [Register("ListController")]
    public class ListController : ReactiveViewController<ListViewModel>
    {
        private static readonly CompositeDisposable ControlBindings = new CompositeDisposable();
        private TableView _tableView;
        private UISegmentedControl _itemType;

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
            _itemType = new UISegmentedControl
            {
                BackgroundColor = UIColor.Clear,
                TintColor = UIColor.Clear
            };

            _itemType.InsertSegment(ItemType.Some.ToString(), (nint)0, true);
            _itemType.InsertSegment(ItemType.Other.ToString(), (nint)1, true);
            _itemType.InsertSegment(ItemType.All.ToString(), (nint)2, true);
            _itemType.SetTitleTextAttributes(
                new UITextAttributes()
                {
                    Font = UIFont.FromName("Arial", 14),
                    TextColor = UIColor.FromRGBA(255, 255, 255, 180),
                }, UIControlState.Normal);

            _itemType.SetTitleTextAttributes(
                new UITextAttributes()
                {
                    Font = UIFont.FromName("Arial", 14),
                    TextColor = UIColor.FromRGBA(255, 255, 255, 255),
                }, UIControlState.Selected);

            _tableView = new TableView()
            {
                DelaysContentTouches = false,
                SeparatorColor = UIColor.Black,
                TableFooterView = new UIView()
            };

            _tableView.RegisterClassForCellReuse(typeof(TableCell), TableCell.ReuseKey);

            View = new UniversalView();
            View.AddSubview(_itemType);
            View.AddSubview(_tableView);
        }

        private void RegisterObservers()
        {
            _itemType
                .Events()
                .ValueChanged
                .Select(x => Convert.ToInt32(_itemType.SelectedSegment))
                .InvokeCommand(this, x => x.ViewModel.ChangeSegment)
                .DisposeWith(ControlBindings);

            var tableViewSource = this.WhenAnyValue(x => x.ViewModel.Items)
                .Select(x => x == null ? null : new TableSource(_tableView, x, TableCell.ReuseKey))
                .Publish();

            tableViewSource
                .BindTo(_tableView, x => x.Source)
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
                .InvokeCommand(this, x => x.ViewModel.LoadData)
                .DisposeWith(ControlBindings);
        }

    }
}