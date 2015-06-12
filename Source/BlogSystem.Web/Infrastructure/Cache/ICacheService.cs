namespace BlogSystem.Web.Infrastructure.Cache
{
    using System;

    public interface ICacheService
    {
        T Get<T>(string itemName, Func<T> getDataFunc, int durationInSeconds);

        void Remove(string itemName);
    }
}