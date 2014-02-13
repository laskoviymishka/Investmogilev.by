using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Investmogilev.Infrastructure.Common.Model.Common;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using StackExchange.Profiling;

namespace Investmogilev.Infrastructure.Common.Repository
{
	public class MongoRepository : IRepository
	{
		#region Private Fields

		private readonly MongoDatabase _db;

		private volatile Dictionary<string, bool> _cacheExpired;

		#endregion

		#region Constructor

		public MongoRepository(string connectionString, string dbName)
		{
			var client = new MongoClient(new MongoUrl(connectionString));
			var server = client.GetServer();
			_db = server.GetDatabase(dbName);
			_cacheExpired = new Dictionary<string, bool>();
		}

		#endregion

		#region Delete

		public void Delete<T>(Expression<Func<T, bool>> expression) where T : class, IMongoEntity
		{
			IQueryable<T> items = All<T>().Where(expression);
			foreach (T item in items)
			{
				Delete(item);
			}
		}

		public void Delete<T>(T item) where T : class, IMongoEntity
		{
			ExpireCacheToken<T>();
			_db.GetCollection(typeof (T).Name).Remove(Query.EQ("ID", new ObjectId(item.Id)));
		}

		public void DeleteAll<T>() where T : class, IMongoEntity
		{
			throw new NotImplementedException();
		}

		#endregion

		#region Selector

		public T GetOne<T>(Expression<Func<T, bool>> expression) where T : class,IMongoEntity
		{
			MiniProfiler profiler = MiniProfiler.Current;
			using (profiler.Step(string.Format("select table {0} query getone {1}", typeof (T).Name, expression)))
			{
				return All<T>().Where(expression).SingleOrDefault();
			}
		}

		public IQueryable<T> All<T>() where T : class, IMongoEntity
		{
			MiniProfiler profiler = MiniProfiler.Current;
			using (profiler.Step(string.Format("select table {0} query {1}", typeof (T).Name, "all")))
			{
				return Get(typeof (T).Name, _db.GetCollection(typeof (T).Name).AsQueryable<T>);
			}
		}

		public IQueryable<T> All<T>(Expression<Func<T, bool>> expression) where T : class, IMongoEntity
		{
			MiniProfiler profiler = MiniProfiler.Current;
			using (profiler.Step(string.Format("select table {0} query All {1}", typeof (T).Name, expression)))
			{
				return All<T>().Where(expression);
			}
		}

		#endregion

		#region Inster

		public void Add<T>(T item) where T : class,IMongoEntity
		{
			ExpireCacheToken<T>();
			_db.GetCollection(typeof (T).Name).Save(item);
		}

		public void Add<T>(IEnumerable<T> items) where T : class,IMongoEntity
		{
			foreach (T item in items)
			{
				Add(item);
			}
		}

		#endregion

		#region Update

		public void Update<T>(T item) where T : class, IMongoEntity
		{
			if (GetOne<T>(t => t.Id == item.Id) != null)
			{
				ExpireCacheToken<T>();
				_db.GetCollection(typeof (T).Name).Save(item);
			}
		}

		#endregion

		#region Cache

		private TC Get<TC>(string cacheId, Func<TC> getItemCallback) where TC : class
		{
			return getItemCallback();
		}

		private void ExpireCacheToken<T>() where T : class, IMongoEntity
		{
			if (!_cacheExpired.ContainsKey(typeof (T).Name))
			{
				_cacheExpired.Add(typeof (T).Name, true);
			}
			else
			{
				_cacheExpired[typeof (T).Name] = true;
			}
		}

		#endregion
	}
}