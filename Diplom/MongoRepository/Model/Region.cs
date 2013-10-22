using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoRepository.Model
{
    public class Region
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
