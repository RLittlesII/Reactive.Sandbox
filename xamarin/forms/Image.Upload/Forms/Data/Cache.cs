using Akavache;
using Forms.Logging;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forms.Data
{
    public class Cache : ICache
    {
        private readonly ILogs _log;
        public Cache(ILogs log = null)
        {
            _log = log ?? Locator.Current.GetService<ILogs>();

            // Make sure you set the application name before doing any inserts or gets
            Akavache.Registrations.Start("UploadManager");
        }

        public async Task Cleanup()
        {
            try
            {
                await BlobCache.LocalMachine.Vacuum();
                await BlobCache.Secure.Vacuum();
            }
            catch (Exception ex)
            {
                _log.Log(ex);
            }
        }

        public async Task Flush()
        {
            try
            {
                await BlobCache.LocalMachine.Flush();
                await BlobCache.Secure.Flush();
            }
            catch (Exception ex)
            {
                _log.Log(ex);
            }
        }

        public async Task<TObj> Get<TObj>(string key)
        {
            try
            {
                var keys = await BlobCache.LocalMachine.GetAllKeys().ToList();
                if (keys.Any(x => x.Contains(key)))
                    return await BlobCache.LocalMachine.GetObject<TObj>(key);
                else
                    return default(TObj);
            }
            catch (Exception ex)
            {
                _log.Log(ex);
                return default(TObj);
            }
        }

        public async Task<TObj> GetSecure<TObj>(string key)
        {
            try
            {
                var keys = await BlobCache.Secure.GetAllKeys().ToList();
                if (keys.Any(x => x.Contains(key)))
                    return await BlobCache.Secure.GetObject<TObj>(key);
                else
                    return default(TObj);
            }
            catch (Exception ex)
            {
                _log.Log(ex);
                return default(TObj);
            }
        }

        public async Task<TObj> Get<TObj>(string key, TObj defaultVal)
        {
            try
            {
                var keys = await BlobCache.LocalMachine.GetAllKeys().ToList();
                if (keys.Any(x => x.Contains(key)))
                    return await BlobCache.LocalMachine.GetObject<TObj>(key);
                else
                    return defaultVal;
            }
            catch (Exception ex)
            {
                _log.Log(ex);
                return defaultVal;
            }
        }

        public async Task<TObj> GetSecure<TObj>(string key, TObj defaultVal)
        {
            try
            {
                var keys = await BlobCache.Secure.GetAllKeys().ToList();
                if (keys.Any(x => x.Contains(key)))
                    return await BlobCache.Secure.GetObject<TObj>(key);
                else
                    return defaultVal;
            }
            catch (Exception ex)
            {
                _log.Log(ex);
                return defaultVal;
            }
        }

        public async Task Insert<TObj>(string key, TObj value, DateTimeOffset? expiration = null)
        {
            try
            {
                await BlobCache.LocalMachine.InsertObject(key, value, expiration);
            }
            catch (Exception ex)
            {
                _log.Log(ex);
            }
        }

        public async Task InsertSecure<TObj>(string key, TObj value, DateTimeOffset? expiration = null)
        {
            try
            {
                await BlobCache.Secure.InsertObject(key, value, expiration);
            }
            catch (Exception ex)
            {
                _log.Log(ex);
            }
        }

        public async Task Invalidate(string key)
        {
            try
            {
                await BlobCache.LocalMachine.Invalidate(key);
            }
            catch (Exception ex)
            {
                _log.Log(ex);
            }
        }

        public async Task InvalidateAll<TObj>()
        {
            try
            {
                await BlobCache.LocalMachine.InvalidateAllObjects<TObj>();
            }
            catch (Exception ex)
            {
                _log.Log(ex);
            }
        }

        public async Task ClearDatabase()
        {
            try
            {
                await BlobCache.LocalMachine.InvalidateAll();
                await BlobCache.Secure.InvalidateAll();
            }
            catch (Exception ex)
            {
                _log.Log(ex);
            }
        }

        public async Task<IEnumerable<string>> GetAllKeys()
        {
            try
            {
                return await BlobCache.LocalMachine.GetAllKeys();
            }
            catch (Exception ex)
            {
                _log.Log(ex);
                return new List<string>();
            }
        }

        public async Task<IEnumerable<T>> GetAllObjects<T>()
        {
            try
            {
                return await BlobCache.LocalMachine.GetAllObjects<T>();
            }
            catch (Exception ex)
            {
                _log.Log(ex);
                return new List<T>();
            }
        }
    }
}
