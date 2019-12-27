using CosmosDB.Cache.Common.Models;
using CosmosDB.Cache.Common.Repository;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

namespace CosmosDB.Cache.Common.Helpers
{
    public class CacheHelper : IDistributedCache
    {
        private readonly IDistributedCache _cache;
        public CacheHelper(IDistributedCache cache)
        {
            _cache = cache;
        }

        public bool IsCacheExist(string key)
        {
            return (Get(key) != null);
        }

        public byte[] ToByteArray<T>(T obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public T FromByteArray<T>(byte[] data)
        {
            if (data == null)
                return default(T);
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(data))
            {
                object obj = bf.Deserialize(ms);
                return (T)obj;
            }
        }
        public byte[] Get(string key)
        {
            return _cache.Get(key);
        }

        public Task<byte[]> GetAsync(string key, CancellationToken token = default)
        {
            return _cache.GetAsync(key, token);
        }

        public void Refresh(string key)
        {
            _cache.Refresh(key);
        }

        public Task RefreshAsync(string key, CancellationToken token = default)
        {
            return _cache.RefreshAsync(key, token);
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        public Task RemoveAsync(string key, CancellationToken token = default)
        {
            return _cache.RemoveAsync(key, token);
        }

        public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            _cache.Set(key, value, options);
        }

        public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            return _cache.SetAsync(key, value, options, token);
        }
    }
}
