using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using CoreGraphics;
using Foundation;
using ReactiveUI;
using UIKit;

namespace ListView
{
    public class TableCell : ReactiveTableViewCell<TableCellViewModel>
    {
        public static NSString ReuseKey = new NSString(nameof(TableCell));

        public CompositeDisposable ViewCellBindings = new CompositeDisposable();

        public UILabel Id { get; set; }

        public UILabel Type { get; set; }

        public UILabel IsToggled { get; set; }
        public TableCell(CGRect frame)
            : base (frame)
        {
        }

        public TableCell(NSString cellId)
            : base (UITableViewCellStyle.Default, cellId)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToFindAssetItemCell"/> class.
        /// </summary>
        /// <param name="handle">The handler.</param>
        protected TableCell(IntPtr handle)
            : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public TableCell(string cellId)
            : base (UITableViewCellStyle.Default, cellId)
        {
            Id = new UILabel(new CGRect(332, -1, 33, 14));

            Type = new UILabel(new CGRect(12, -1, 324, 14)) { Font = UIFont.SystemFontOfSize(13f) };

            IsToggled = new UILabel(new CGRect(12, 18, 116, 8)) { Font = UIFont.SystemFontOfSize(9f) };

            this.OneWayBind(ViewModel, x => x.Id, x => x.Id.Text)
                .DisposeWith(ViewCellBindings);

            this.OneWayBind(ViewModel, x => x.Type, x => x.Type.Text)
                .DisposeWith(ViewCellBindings);

            this.OneWayBind(ViewModel, x => x.IsToggled, x => x.IsToggled.Text)
                .DisposeWith(ViewCellBindings);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            ContentView.Frame = new CGRect(0, 0, 376, 43.5);
            ContentView.AddSubviews(Id, Type, IsToggled);
        }
    }
}