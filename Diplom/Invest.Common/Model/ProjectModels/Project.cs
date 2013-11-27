using System.Collections.Generic;
using Invest.Common.Model.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Invest.Common.Model.ProjectModels
{
    [BsonKnownTypes(typeof(GreenField), typeof(BrownField), typeof(UnUsedBuilding))]
    public class Project : IMongoEntity
    {
        private string _investuser;
        [BsonRepresentation(BsonType.ObjectId)]
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
        public string AssignUser { get; set; }
        public string WorkflowId { get; set; }
        public string InvestorUser
        {
            get
            {
                if (_investuser == null)
                {
                    return string.Empty;
                }
                return _investuser;
            }
            set { _investuser = value; }
        }

        public IList<Task> Tasks { get; set; } 

        public WorkflowEntity WorkflowState { get; set; }

        [BsonIgnore]
        public string ProjectType
        {
            get { return this.GetType().Name; }
        }

        [BsonIgnore]
        public string CurrenState
        {
            get { return "test"; }
        }

        public IList<InvestorResponse> Responses { get; set; }

    }
}
