using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;

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
            this.OneWayBind(ViewModel, x => x.Id, x => x.Id.Text)
                .DisposeWith(ViewCellBindings);

            this.OneWayBind(ViewModel, x => x.Type, x => x.Type.Text)
                .DisposeWith(ViewCellBindings);

            this.OneWayBind(ViewModel, x => x.IsToggled, x => x.IsToggled.Text)
                .DisposeWith(ViewCellBindings);
        }
    }
}