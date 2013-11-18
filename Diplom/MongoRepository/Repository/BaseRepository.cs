using System.Linq;
using Invest.Common.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace MongoRepository.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : MongoEntity
    {
        #region Fields

        private readonly MongoClient _client;

        private readonly MongoServer _server;

        private readonly MongoDatabase _db;

        private readonly string _collectionName;

        #endregion

        #region Constructor

        public BaseRepository(string connectionString, string dbName, string collectionName)
        {
            _client = new MongoClient(new MongoUrl(connectionString));
            _server = _client.GetServer();
            _db = _server.GetDatabase(dbName);
            _collectionName = collectionName;
        }

        #endregion

        public System.Collections.Generic.IList<T> GetAll()
        {
            return _db.GetCollection(_collectionName).AsQueryable<T>().ToList();
        }

        public T GetById(string id)
        {
            ObjectId _id = new ObjectId(id);
            return _db.GetCollection(_collectionName).FindOneAs<T>(Query.EQ("_id", _id));
        }

        public void Insert(T value)
        {
            _db.GetCollection(_collectionName).Insert(value);
        }

        public void Update(T value)
        {
            if (this.GetById(value._id) != null)
            {
                _db.GetCollection(_collectionName).Save<T>(value);
            }
        }

        public void Delete(T value)
        {
            if (this.GetById(value._id) != null)
            {
                _db.GetCollection(_collectionName).Remove(Query.EQ("_id", value._id));
            }
        }
    }
}