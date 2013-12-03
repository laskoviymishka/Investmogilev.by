using System;
using System.Collections.Generic;
using Invest.Common.Model.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Invest.Common.Model.ProjectModels
{
    public class ProjectNotes : IMongoEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public string ProjectId { get; set; }

        public string NoteTitle { get; set; }

        public string NoteBody { get; set; }

        public string CretorName { get; set; }

        public DateTime CreatedTime { get; set; }

        public List<string> RolesForView { get; set; }

        public IEnumerable<AdditionalInfo> NoteDocument { get; set; }
    }
}