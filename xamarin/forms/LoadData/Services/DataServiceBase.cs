using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using DynamicData;

namespace Services
{
    public class DataServiceBase<T> : IDataService<T>
        where T : Dto
    {
        protected readonly IHubClient HubClient;
        protected readonly SourceCache<T, Guid> SourceCache = new SourceCache<T, Guid>(x => x.Id);

        protected DataServiceBase(IHubClient hubClient)
        {
            HubClient = hubClient;
        }

        public IObservableCache<T, Guid> DataCache => SourceCache;

        public IObservable<IChangeSet<T, Guid>> DataSource => SourceCache.Connect().Publish().RefCount();

        public async Task<T> Get(Guid id)
        {
            await HubClient.Connect();
            var optional = SourceCache.Lookup(id);
            return optional.HasValue ? optional.Value : default;
        }

        public async Task GetAll() => await Task.CompletedTask;

        public async Task Save(T entity) => SourceCache.AddOrUpdate(entity);

        public async Task Delete(T entity) => SourceCache.Remove(entity.Id);
    }
}