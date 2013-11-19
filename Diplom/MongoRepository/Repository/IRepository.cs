using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Invest.Common.Model;

namespace MongoRepository.Repository
{
    public interface IRepository
    {
        void Delete<T>(Expression<Func<T, bool>> expression) where T : MongoEntity;
        void Delete<T>(T item) where T : MongoEntity;
        void DeleteAll<T>() where T : MongoEntity;
        T GetById<T>(Expression<Func<T, bool>> expression) where T : MongoEntity;
        IQueryable<T> All<T>() where T : MongoEntity;
        void Add<T>(T item) where T : MongoEntity;
        void Add<T>(IEnumerable<T> items) where T : MongoEntity;
        void Update<T>(T item) where T : MongoEntity;
    }
}
