using System;
using System.Collections.Generic;
using Invest.Common.Model.Common;
using Invest.Common.State;

namespace Invest.Common.Model.Project
{
    public class ProjectTask : IMongoEntity
    {
        public string _id { get; set; }
        public string ProjectId { get; set; }
        public string Title { get; set; }
        public List<Report> TaskReport { get; set; }
        public TaskTypes Type { get; set; }
        public ProjectWorkflow.State Step { get; set; } // Задача привязана к статусу проекта
        public DateTime Milestone { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime CompleteTime { get; set; }
        public bool IsComplete { get; set; }
        public string Body { get; set; }
    }
}
