using Invest.Common.Model.Common;
using Invest.Common.State;

namespace Invest.Common.Model.Project
{
    public class TaskTemplate : IMongoEntity
    {
        public string _id { get; set; }
        public string Title { get; set; }
        public TaskTypes Type { get; set; }
        public ProjectWorkflow.State Step { get; set; }
        public string Body { get; set; }
        public AdditionalInfo Info { get; set; }
    }
}