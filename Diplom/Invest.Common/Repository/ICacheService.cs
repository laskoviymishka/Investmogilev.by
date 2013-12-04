using System;
namespace Invest.Common.Repository
{
    public interface ICacheService
    {
        T Get<T>(string cacheId, Func<T> getItemCallback) where T : class;
    }
}