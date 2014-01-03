using System;
using System.Collections.Generic;
using System.Linq;
using Invest.Common.Model.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Invest.Common.Model.Project
{
    public class Comission : IMongoEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public DateTime CommissionTime { get; set; }

        public List<string> ProjectIds { get; set; }

        [BsonIgnore]
        public List<Project> Projects {
            get { return RepositoryContext.Current.All<Project>(p => ProjectIds.Contains(p._id)).ToList(); }
        }
    }
}