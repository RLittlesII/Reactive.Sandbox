using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using DynamicData;

namespace Removeable
{
    public class DataServiceBase<T> : IDataService<T>
        where T : Dto
    {
        private readonly IClient _client;
        protected readonly SourceCache<T, Guid> SourceCache = new SourceCache<T, Guid>(x => x.Id);

        protected DataServiceBase(IClient client)
        {
            _client = client;
        }

        /// <inheritdoc />
        public IObservable<IChangeSet<T, Guid>> ChangeSet => SourceCache.Connect().RefCount();

        /// <inheritdoc />
        public IObservable<Unit> Create(T dto) =>
            _client
                .Post(dto)
                .ToObservable()
                .Do(_ => SourceCache.AddOrUpdate(_))
                .Select(x => Unit.Default);

        /// <inheritdoc />
        public IObservable<T> Read() =>
            _client
                .GetAll<T>()
                .ToObservable()
                .SelectMany(x => x)
                .WhereNotNull()
                .Do(_ => SourceCache.AddOrUpdate(_));

        /// <inheritdoc />
        public IObservable<T> Read(Guid id) =>
            _client
                .Get<T>(id)
                .ToObservable()
                .WhereNotNull()
                .Do(_ => SourceCache.AddOrUpdate(_));

        /// <inheritdoc />
        public IObservable<Unit> Update(T dto) =>
            _client
                .Post(dto)
                .ToObservable()
                .WhereNotNull()
                .Do(_ => SourceCache.AddOrUpdate(dto))
                .ToSignal();

        /// <inheritdoc />
        public IObservable<Unit> Delete(T dto) =>
            _client
                .Delete(dto)
                .ToObservable()
                .WhereNotNull()
                .Do(_ => SourceCache.Remove(dto))
                .ToSignal();

        IObservable<Unit> IDataService.Create(object dto) => Create((T) dto);

        IObservable<object> IDataService.Read() => Read();

        IObservable<object> IDataService.Read(Guid id) => Read(id);

        IObservable<Unit> IDataService.Update(object dto) => Update((T) dto);

        IObservable<Unit> IDataService.Delete(object dto) => Delete((T) dto);

        public IObservable<Unit> Delete(Guid id)
        {
            var dto = SourceCache.Lookup(id);
            return _client
                .Delete(dto.Value)
                .ToObservable()
                .WhereNotNull()
                .Do(_ => SourceCache.Remove(id))
                .ToSignal();
        }
    }
}