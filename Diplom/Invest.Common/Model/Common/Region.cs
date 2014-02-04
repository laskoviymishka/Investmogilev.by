using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Investmogilev.Infrastructure.Common.Model.Common
{
    public class Region : IMongoEntity
    {
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string _id
        {
            get;
            set;
        }

        public string RegionName
        {
            get;
            set;
        }

        public string EnglishName
        {
            get;
            set;
        }

        public IList<AdditionalInfo> MoreInfo
        {
            get;
            set;
        }

        public IList<Parametrs> Parametrs
        {
            get;
            set;
        }
    }
}
