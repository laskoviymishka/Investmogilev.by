using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Invest.Common.Model
{
    public interface MongoEntity
    {
        string _id { get; set; }
    }
}
