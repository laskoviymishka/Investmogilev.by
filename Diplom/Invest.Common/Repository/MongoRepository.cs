using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Invest.Common.Model;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace Invest.Common.Repository
{
    public class MongoRepository : IRepository
    {
        #region Private Fields

        private readonly MongoClient _client;

        private readonly MongoServer _server;

        private readonly MongoDatabase _db;

        #endregion

        #region Constructor

        public MongoRepository(string connectionString, string dbName)
        {
            _client = new MongoClient(new MongoUrl(connectionString));
            _server = _client.GetServer();
            _db = _server.GetDatabase(dbName);
        }

        #endregion

        #region Delete

        public void Delete<T>(Expression<Func<T, bool>> expression) where T : MongoEntity
        {
            var items = All<T>().Where(expression);
            foreach (T item in items)
            {
                Delete(item);
            }
        }

        public void Delete<T>(T item) where T : MongoEntity
        {
            _db.GetCollection(typeof(T).Name).Remove(Query.EQ("_id", item._id));
        }

        public void DeleteAll<T>() where T : MongoEntity
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Selector

        public T GetOne<T>(Expression<Func<T, bool>> expression) where T : MongoEntity
        {
            return All<T>().Where(expression).SingleOrDefault();
        }

        public IQueryable<T> All<T>() where T : MongoEntity
        {
            return _db.GetCollection(typeof(T).Name).AsQueryable<T>();
        }

        public IQueryable<T> All<T>(Expression<Func<T, bool>> expression) where T : MongoEntity
        {
            return _db.GetCollection(typeof (T).Name).AsQueryable<T>().Where(expression);
        }

        #endregion

        #region Inster

        public void Add<T>(T item) where T : MongoEntity
        {
            _db.GetCollection(typeof(T).Name).Save(item);
        }

        public void Add<T>(IEnumerable<T> items) where T : MongoEntity
        {
            foreach (T item in items)
            {
                Add(item);
            }
        }

        #endregion

        #region Update

        public void Update<T>(T item) where T : MongoEntity
        {
            if (this.GetOne<T>(t=> t._id == item._id) != null)
            {
                _db.GetCollection(typeof(T).Name).Save<T>(item);
            }
        } 

        #endregion
    }
}