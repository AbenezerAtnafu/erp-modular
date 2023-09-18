using StackExchange.Redis;

namespace ERP.Interface
{
    public interface ICacheService
    {
        string GetData(string key);

        bool SetData<T> (string key, T value, DateTimeOffset expirationTime);
        object RemoveData (string key);

    }
}
