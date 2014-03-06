using System;

namespace Investmogilev.Infrastructure.Common.Repository
{
	public interface ICacheService
	{
		T Get<T>(string cacheId, Func<T> getItemCallback) where T : class;
	}
}