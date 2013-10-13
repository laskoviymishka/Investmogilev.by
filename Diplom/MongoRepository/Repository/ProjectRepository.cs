using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;
using MongoRepository.Model;

namespace MongoRepository.Repository
{
    public class ProjectRepository
    {
        #region Fields

        private readonly MongoClient _client;

        private readonly MongoServer _server;

        private readonly MongoDatabase _db;

        private MongoCollection<BsonDocument> _collection;

        #endregion

        #region Constructor

        public ProjectRepository()
        {
            _client = new MongoClient(new MongoUrl("mongodb://tserakhau.cloudapp.net"));
            _server = _client.GetServer();
            _db = _server.GetDatabase("Projects");
        }

        #endregion

        #region Selectors

        public IList<Project> AllProjects()
        {
            List<Project> result = new List<Project>();
            result.AddRange(this.GetBrownFieldsProject());
            result.AddRange(this.GetUnUsedBuildingProject());
            result.AddRange(this.GetGreenFieldProject());
            return result;
        }

        public IList<BrownField> GetBrownFieldsProject() 
        {
            return _db.GetCollection("BrownField").AsQueryable<BrownField>().ToList();
        }

        public IList<UnUsedBuilding> GetUnUsedBuildingProject()
        {
            return _db.GetCollection("UnUsedBuilding").AsQueryable<UnUsedBuilding>().ToList();
        }

        public IList<GreenField> GetGreenFieldProject()
        {
            return _db.GetCollection("GreenField").AsQueryable<GreenField>().ToList();
        }

        public T GetProjectByID<T>(string id) where T : Project
        {
            ObjectId _id = new ObjectId(id);
            return _db.GetCollection(typeof(T).Name).FindOneAs<T>(Query.EQ("_id", _id));
        }

        public T GetProjectByID<T>(ObjectId id) where T : Project
        {
            return _db.GetCollection(typeof(T).Name).FindOneAs<T>(Query.EQ("_id", id));
        }

        #endregion

        #region Insert

        public void InsertProject(BrownField project)
        {
            _db.GetCollection("BrownField").Insert<BrownField>(project);
        }

        public void InsertProject(GreenField project) 
        {
            _db.GetCollection("GreenField").Insert<GreenField>(project);
        }

        public void InsertProject(UnUsedBuilding project)
        {
            _db.GetCollection("UnUsedBuilding").Insert<UnUsedBuilding>(project);
        }

        #endregion

        #region Update

        public void UpdateOne<T>(T value) where T : Project
        {
            if (this.GetProjectByID<T>(value._id) != null) 
            {
                _db.GetCollection(typeof(T).Name).Save<T>(value);
            }
        }

        #endregion
    }
}
