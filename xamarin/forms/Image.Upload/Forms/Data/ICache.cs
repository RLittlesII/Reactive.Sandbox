using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forms.Data
{
    public interface ICache
    {
        Task Cleanup();
        Task ClearDatabase();
        Task Flush();
        Task<TObj> Get<TObj>(string key);
        Task<TObj> Get<TObj>(string key, TObj defaultVal);
        Task<IEnumerable<string>> GetAllKeys();
        Task<IEnumerable<T>> GetAllObjects<T>();
        Task<TObj> GetSecure<TObj>(string key);
        Task<TObj> GetSecure<TObj>(string key, TObj defaultVal);
        Task Insert<TObj>(string key, TObj value, DateTimeOffset? expiration = null);
        Task InsertSecure<TObj>(string key, TObj value, DateTimeOffset? expiration = null);
        Task Invalidate(string key);
        Task InvalidateAll<TObj>();
    }
}