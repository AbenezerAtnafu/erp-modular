using ERP.Interface;
using StackExchange.Redis;
using System.Text.Json;

namespace ERP.Service
{
    public class CacheService : ICacheService
    {
        IDatabase _cacheDb;
        public CacheService()
        {
            var redis = ConnectionMultiplexer.Connect("localhost:6379");
            _cacheDb=redis.GetDatabase();
        }
        public string GetData(string key)
        {
            var value = _cacheDb.StringGet(key);
            return value;
        }

        public object RemoveData(string key)
        {
            var _exist=_cacheDb.KeyExists(key);
            if(_exist)
            {
                return _cacheDb.KeyDelete(key); 

            }

            return false;
        }

        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var expiry = expirationTime.Subtract(DateTimeOffset.Now);
            var serializedValue = JsonSerializer.Serialize(value);
            return _cacheDb.StringSet(key, serializedValue, expiry);
        }
    }
}
