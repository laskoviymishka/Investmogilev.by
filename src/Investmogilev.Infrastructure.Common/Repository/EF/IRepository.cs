using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Investmogilev.Infrastructure.Common.Model.Common;

namespace Investmogilev.Infrastructure.Common.Repository.EF
{
	public interface IGRepository<T> where T : IMongoEntity
	{
		void Delete<T>(Expression<Func<T, bool>> expression);
		void Delete<T>(T item);
		void DeleteAll<T>();
		T GetOne<T>(Expression<Func<T, bool>> expression);
		IQueryable<T> All<T>(Expression<Func<T, bool>> expression);
		IQueryable<T> All<T>();
		void Add<T>(T item);
		void Add<T>(IEnumerable<T> items);
		void Update<T>(T item);
	}
}