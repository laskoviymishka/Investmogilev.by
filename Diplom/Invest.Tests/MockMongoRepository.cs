using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Invest.Common.Model.Common;
using Invest.Common.Repository;

namespace Invest.Tests
{
    public class MockMongoRepository : IRepository
    {
        #region Private Fields

        private readonly IList _db;

        #endregion

        #region Constructor

        public MockMongoRepository(IList data)
        {
            _db = data;
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
            (_db as IList<T>).Remove(item);
        }

        public void DeleteAll<T>() where T : IMongoEntity
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Selector

        public T GetOne<T>(Expression<Func<T, bool>> expression) where T : IMongoEntity
        {
            return All<T>().Where(expression).SingleOrDefault();
        }

        public IQueryable<T> All<T>() where T : IMongoEntity
        {
            return (_db as IList<T>).AsQueryable();
        }

        public IQueryable<T> All<T>(Expression<Func<T, bool>> expression) where T : IMongoEntity
        {
            return All<T>().Where(expression);
        }

        #endregion

        #region Inster

        public void Add<T>(T item) where T : IMongoEntity
        {
            (_db as IList<T>).Add(item);
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
                T elem = GetOne<T>(t => t._id == item._id);
                Delete<T>(item);
                Add<T>(item);
            }
        }

        #endregion
    }
}