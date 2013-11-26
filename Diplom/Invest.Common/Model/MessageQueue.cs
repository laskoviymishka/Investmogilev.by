using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Invest.Common.Model
{
    public class MessageQueue : MongoEntity
    {
        private ObjectId _objectId;

        [BsonRepresentation(BsonType.ObjectId)]
        public string _id
        {
            get { return _objectId.ToString(); }
            set { _objectId = ObjectId.Parse(value); }
        }

        public string Title { get; set; }

        public MessageBody Body { get; set; }

        public DateTime SendedDate { get; set; }

        public Recepient Sender { get; set; }

        public Recepient Recepient { get; set; }

        public bool IsSended { get; set; }

        public bool IsReaded { get; set; }

        public string ReplayTo { get; set; }
    }
}