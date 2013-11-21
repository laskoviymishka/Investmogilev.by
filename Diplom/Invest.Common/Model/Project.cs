using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;

namespace Invest.Common.Model
{
    public class Project : MongoEntity
    {
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

        [BsonIgnore]
        public string ProjectType
        {
            get { return this.GetType().Name; }
        }

        [BsonIgnore]
        public IEnumerable<InvestorResponse> Responses
        {
            get { return RepositoryContext.Current.All<InvestorResponse>(r => r.ResponsedProjectId == _id); }
        }

        [BsonIgnore]
        public WorkflowEntity WorkflowState
        {
            get
            {
                if (RepositoryContext.Current.GetOne<WorkflowEntity>(w => w.ProjectId == _id) == null)
                {
                    WorkflowEntity we = new WorkflowEntity();
                    we.CurrenState = "Open";
                    we.ProjectId = _id;
                    we.ChangeHistory = new List<History>();
                    RepositoryContext.Current.Add(we);
                }

                return RepositoryContext.Current.GetOne<WorkflowEntity>(w => w.ProjectId == _id);
            }
        }
    }
}
