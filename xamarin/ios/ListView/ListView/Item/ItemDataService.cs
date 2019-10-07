using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using DynamicData;
using Splat;

namespace ListView
{
    public class ItemDataService : IEnableLogger
    {
        private readonly SourceCache<Item, Guid> _source;

        public ItemDataService()
        {
            try
            {
                _source = new SourceCache<Item, Guid>(item => item.Id);

                ChangedItems = _source.Connect().RefCount().SubscribeOn(TaskPoolScheduler.Default);
                var items = Enumerable.Repeat(new Item(), 30).ToArray();
                _source.AddOrUpdate(items);

            }
            catch (Exception exception)
            {
                this.Log().Error(exception);
                throw;
            }
        }

        public IObservable<IChangeSet<Item, Guid>> ChangedItems { get; }

        public void Add(Item item) => _source.Edit(innerList => innerList.AddOrUpdate(item));

        public void Add(IEnumerable<Item> item) => _source.Edit(innerList => innerList.AddOrUpdate(item));
    }
}