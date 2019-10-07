using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

using Foundation;
using ReactiveUI;
using UIKit;

namespace ListView
{
    public class TableSource : ReactiveTableViewSource<TableCellViewModel>
    {
        public TableSource(UITableView tableView,
            INotifyCollectionChanged collection,
            NSString cellKey,
            float sizeHint = 64,
            Action<UITableViewCell> initializeCellAction = null) : base(tableView,
            collection,
            cellKey,
            sizeHint,
            initializeCellAction)
        {
        }

        public TableSource(UITableView tableView,
            IReadOnlyList<TableSectionInformation<TableCellViewModel>> sectionInformation)
            : base(tableView, sectionInformation)
        {
        }

        public TableSource(UITableView tableView) : base(tableView)
        {
        }
    }
}