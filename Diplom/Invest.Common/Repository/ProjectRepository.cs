using System;
using System.Collections.Generic;
using System.Linq;
using Invest.Common.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace Invest.Common.Repository
{
    public class ProjectRepository
    {
        #region Fields

        private readonly MongoClient _client;

        private readonly MongoServer _server;

        private readonly MongoDatabase _db;

        private MongoCollection<BsonDocument> _collection;

        private readonly IRepository _mongorepository;

        #endregion

        #region Constructor

        public ProjectRepository()
        {
            _client = new MongoClient(new MongoUrl("mongodb://tserakhau.cloudapp.net"));
            _server = _client.GetServer();
            _db = _server.GetDatabase("Projects");
            _mongorepository = new MongoRepository("mongodb://tserakhau.cloudapp.net", "Projects");
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

        public IQueryable<BrownField> GetBrownFieldsProject() 
        {
            return _mongorepository.All<BrownField>();
        }

        public IQueryable<UnUsedBuilding> GetUnUsedBuildingProject()
        {
            return _mongorepository.All<UnUsedBuilding>();
        }

        public IQueryable<GreenField> GetGreenFieldProject()
        {
            return _mongorepository.All<GreenField>();
        }

        public T GetProjectByID<T>(string id) where T : Project
        {
            return _mongorepository.GetOne<T>(t => t._id == id);
        }

        public T GetProjectByID<T>(ObjectId id) where T : Project
        {
            return _db.GetCollection(typeof(T).Name).FindOneAs<T>(Query.EQ("_id", id));
        }

        #endregion

        #region Insert

        public void InsertProject(BrownField project)
        {
            _mongorepository.Add(project);
        }

        public void InsertProject(GreenField project) 
        {
            _mongorepository.Add(project);
        }

        public void InsertProject(UnUsedBuilding project)
        {
            _mongorepository.Add(project);
        }

        #endregion

        #region Update

        public void UpdateOne<T>(T value) where T : Project
        {
            if (this.GetProjectByID<T>(value._id) != null) 
            {
               _mongorepository.Update(value);
            }
        }

        #endregion

        #region Delete

        public void Delete(BrownField project)
        {
            if (this.GetProjectByID<BrownField>(project._id) != null)
            {
                _db.GetCollection("BrownField").Remove(Query.EQ("_id", project._id));
            }
        }

        public void Delete(GreenField project)
        {
            if (this.GetProjectByID<GreenField>(project._id) != null)
            {
                _db.GetCollection("GreenField").Remove(Query.EQ("_id", project._id));
            }
        }

        public void Delete(UnUsedBuilding project)
        {
            if (this.GetProjectByID<UnUsedBuilding>(project._id) != null)
            {
                _db.GetCollection("UnUsedBuilding").Remove(Query.EQ("_id", project._id));
            }
        }

        #endregion
    }
}
