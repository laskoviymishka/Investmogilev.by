using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Investmogilev.Infrastructure.Common.Model.Common;

namespace Investmogilev.Infrastructure.Common.Repository
{
	public interface IRepository
	{
		void Delete<T>(Expression<Func<T, bool>> expression) where T : class, IMongoEntity;
		void Delete<T>(T item) where T : class, IMongoEntity;
		void DeleteAll<T>() where T : class,IMongoEntity;
		T GetOne<T>(Expression<Func<T, bool>> expression) where T : class, IMongoEntity;
		IQueryable<T> All<T>(Expression<Func<T, bool>> expression) where T : class, IMongoEntity;
		IQueryable<T> All<T>() where T : class, IMongoEntity;
		void Add<T>(T item) where T : class, IMongoEntity;
		void Add<T>(IEnumerable<T> items) where T : class, IMongoEntity;
		void Update<T>(T item) where T : class, IMongoEntity;
	}
}