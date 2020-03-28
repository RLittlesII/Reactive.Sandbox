using System;
using System.Threading.Tasks;
using DynamicData;

namespace Services
{
    public interface IDataService<T>
        where T : Dto
    {
        IObservableCache<T, Guid> DataCache { get; }

        IObservable<IChangeSet<T, Guid>> DataSource { get; }

        Task<T> Get(Guid id);

        Task GetAll();

        Task Save(T entity);

        Task Delete(T entity);
    }

    public class Dto
    {
        public Guid Id { get; set; }
    }
}