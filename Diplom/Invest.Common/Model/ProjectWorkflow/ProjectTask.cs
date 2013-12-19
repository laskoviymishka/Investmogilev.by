using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Invest.Common.Model.Common;

namespace Invest.Common.Model.ProjectWorkflow
{
    public class ProjectTask : IMongoEntity
    {
        public string Title { get; set; }
        public IEnumerable<TaskReport> TaskReport { get; set; }
        public TaskTypes Type { get; set;}
        public TaskStep Step { get; set;} // Задача привязана к статусу проекта
        public DateTime CreationTime { get; set; }
        public DateTime CompleteTime { get; set; }
        public bool IsComplete { get; set; }
        public string Body { get; set; }
        public string InvolvedOrganization { get; set; }
        public string _id { get; set; }
    }
}
