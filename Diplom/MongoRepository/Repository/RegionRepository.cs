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
    public class RegionRepository
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

        public IList<Region> AllRegion()
        {
            return _db.GetCollection("Region").FindAllAs<Region>().ToList();
        }

        public IList<string> AllRegionNames()
        {
            List<string> result = new List<string>();
            foreach (var item in this.AllRegion())
            {
                result.Add(item.RegionName);
            }
            return result;
        }

        public Region GetRegionByID(string id)
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

        public void InsertRegion(Region region)
        {
            _db.GetCollection("Region").Insert<Region>(region);
        }

        #endregion

        #region Update

        public void UpdateOne<T>(T value) where T : Region
        {
            if (this.GetRegionByID(value._id) != null)
            {
                _db.GetCollection(typeof(T).Name).Save<T>(value);
            }
            _db.GetCollection(typeof(T).Name).Save<T>(value);
        }

        #endregion
    }
}
