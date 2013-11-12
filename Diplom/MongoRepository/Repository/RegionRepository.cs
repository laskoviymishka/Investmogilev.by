using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoRepository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoRepository.Repository
{
    public class RegionRepository : IRepository<Region>
    {
        #region Fields

        private readonly MongoClient _client;

        private readonly MongoServer _server;

        private readonly MongoDatabase _db;

        #endregion

        #region Constructor

        public RegionRepository()
        {
            _client = new MongoClient(new MongoUrl("mongodb://tserakhau.cloudapp.net"));
            _server = _client.GetServer();
            _db = _server.GetDatabase("Projects");
        }

        #endregion

        #region Selectors

        public IList<Region> GetAll()
        {
            return _db.GetCollection("Region").FindAllAs<Region>().ToList();
        }

        public IList<string> AllRegionNames()
        {
            List<string> result = new List<string>();
            foreach (var item in this.GetAll())
            {
                result.Add(item.RegionName);
            }
            return result;
        }

        public Region GetById(string id)
        {
            ObjectId _id = new ObjectId(id);
            return _db.GetCollection(typeof(Region).Name).FindOneAs<Region>(Query.EQ("_id", _id));
        }

        public Region GetRegionByID(ObjectId id)
        {
            return _db.GetCollection(typeof(Region).Name).FindOneAs<Region>(Query.EQ("_id", id));
        }

        #endregion

        #region Insert

        public void Insert(Region region)
        {
            _db.GetCollection("Region").Insert<Region>(region);
        }

        #endregion

        #region Update

        public void Update(Region value) 
        {
            if (this.GetById(value._id) != null)
            {
                _db.GetCollection(typeof(Region).Name).Save<Region>(value);
            }
            _db.GetCollection(typeof(Region).Name).Save<Region>(value);
        }

        #endregion

        #region Delete

        public void Delete(Region value)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
