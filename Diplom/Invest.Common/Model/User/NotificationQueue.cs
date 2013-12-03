using System;
using Invest.Common.Model.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Invest.Common.Model.User
{
    public class NotificationQueue : IMongoEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public string UserName { get; set; }

        public string NotificationTitle { get; set; }

        public string NotigicationBody { get; set; }

        public bool IsRead { get; set; }

        public DateTime NotificationTime { get; set; }
    }
}