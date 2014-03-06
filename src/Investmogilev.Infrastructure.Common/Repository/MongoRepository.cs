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

		public void Delete<T>(Expression<Func<T, bool>> expression) where T : IMongoEntity
		{
			IQueryable<T> items = All<T>().Where(expression);
			foreach (T item in items)
			{
				Delete(item);
			}
		}

		public void Delete<T>(T item) where T : IMongoEntity
		{
			ExpireCacheToken<T>();
			_db.GetCollection(typeof (T).Name).Remove(Query.EQ("_id", new ObjectId(item._id)));
		}

		public void DeleteAll<T>() where T : IMongoEntity
		{
			throw new NotImplementedException();
		}

		#endregion

		#region Selector

		public T GetOne<T>(Expression<Func<T, bool>> expression) where T : IMongoEntity
		{
			MiniProfiler profiler = MiniProfiler.Current;
			using (profiler.Step(string.Format("select table {0} query getone {1}", typeof (T).Name, expression)))
			{
				return All<T>().Where(expression).SingleOrDefault();
			}
		}

		public IQueryable<T> All<T>() where T : IMongoEntity
		{
			MiniProfiler profiler = MiniProfiler.Current;
			using (profiler.Step(string.Format("select table {0} query {1}", typeof (T).Name, "all")))
			{
				return Get(typeof (T).Name, _db.GetCollection(typeof (T).Name).AsQueryable<T>);
			}
		}

		public IQueryable<T> All<T>(Expression<Func<T, bool>> expression) where T : IMongoEntity
		{
			MiniProfiler profiler = MiniProfiler.Current;
			using (profiler.Step(string.Format("select table {0} query All {1}", typeof (T).Name, expression)))
			{
				return All<T>().Where(expression);
			}
		}

		#endregion

		#region Inster

		public void Add<T>(T item) where T : IMongoEntity
		{
			ExpireCacheToken<T>();
			_db.GetCollection(typeof (T).Name).Save(item);
		}

		public void Add<T>(IEnumerable<T> items) where T : IMongoEntity
		{
			foreach (T item in items)
			{
				Add(item);
			}
		}

		#endregion

		#region Update

		public void Update<T>(T item) where T : IMongoEntity
		{
			if (GetOne<T>(t => t._id == item._id) != null)
			{
				ExpireCacheToken<T>();
				_db.GetCollection(typeof (T).Name).Save(item);
			}
		}

		#endregion

		#region Cache

		private TC Get<TC>(string cacheId, Func<TC> getItemCallback) where TC : class
		{
			var item = HttpRuntime.Cache.Get(cacheId) as TC;
			if (item == null)
			{
				item = getItemCallback();
				HttpContext.Current.Cache.Insert(cacheId, item);
				if (!_cacheExpired.ContainsKey(cacheId))
				{
					_cacheExpired.Add(cacheId, false);
				}
				else
				{
					_cacheExpired[cacheId] = false;
				}
			}
			else
			{
				if (_cacheExpired.ContainsKey(cacheId))
				{
					if (!_cacheExpired[cacheId])
					{
						return item;
					}

					item = getItemCallback();
					HttpContext.Current.Cache.Insert(cacheId, item);
					_cacheExpired[cacheId] = false;
				}
				else
				{
					_cacheExpired.Add(cacheId, true);
				}
			}

			return getItemCallback();
		}

		private void ExpireCacheToken<T>() where T : IMongoEntity
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