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

        public TableCell()
        {
            Id = new UILabel(new CGRect(332, -1, 33, 14));

            Type = new UILabel(new CGRect(12, -1, 324, 14)) {Font = UIFont.SystemFontOfSize(13f)};

            IsToggled = new UILabel(new CGRect(12, 18, 116, 8)) {Font = UIFont.SystemFontOfSize(9f)};

            this.OneWayBind(ViewModel, x => x.Id, x => x.Id.Text)
                .DisposeWith(ViewCellBindings);

            this.OneWayBind(ViewModel, x => x.Type, x => x.Type.Text)
                .DisposeWith(ViewCellBindings);

            this.OneWayBind(ViewModel, x => x.IsToggled, x => x.IsToggled.Text)
                .DisposeWith(ViewCellBindings);
        }
    }
}