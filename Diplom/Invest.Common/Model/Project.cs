using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Invest.Common.Model
{
    public class Project : MongoEntity
    {

        public string _id
        {
            get;
            set;
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public Address Address { get; set; }
        public string AddressName { get; set; }
        public string Contact { get; set; }
        public string Region { get; set; }
        public string[] Mentors { get; set; }
        public string WorkflowId { get; set; }
    }
}
