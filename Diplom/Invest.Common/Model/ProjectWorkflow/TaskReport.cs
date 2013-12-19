using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Invest.Common.Model.Common;

namespace Invest.Common.Model.ProjectWorkflow
{
    public class TaskReport : IMongoEntity
    {
        public string Body { get; set; }
        public IEnumerable<AdditionalInfo> Info { get; set; }
        public TaskResponse Response { get; set; }

        public string _id { get; set; }
    }
}
