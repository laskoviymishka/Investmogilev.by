using System.Collections.Generic;
using Invest.Common.Model.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;

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

        [BsonIgnore]
        public IEnumerable<Task> Tasks
        {
            get
            {
                return RepositoryContext.Current.All<Task>(t => t.ParentId == _id);
            }
        }

        public WorkflowEntity WorkflowState { get; set; }

        [BsonIgnore]
        public string ProjectType
        {
            get { return this.GetType().Name; }
        }

        public IList<InvestorResponse> Responses { get; set; }

        [BsonIgnore]
        public IEnumerable<ProjectNotes> Notes
        {
            get
            {
                return RepositoryContext.Current.All<ProjectNotes>(t => t.ProjectId == _id);
            }
        }
    }
}
