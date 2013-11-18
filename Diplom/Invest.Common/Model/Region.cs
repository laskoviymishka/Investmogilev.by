using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Invest.Common.Model
{
    public class Region : MongoEntity
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

        public IList<AdditionalRegionInfo> MoreInfo
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
