using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Invest.Common.Model.Common;

namespace Invest.Common.Model.ProjectModels
{
    public class TaskReport : IMongoEntity
    {
        public string _id { get; set; }
        public string Body { get; set; }
        public IEnumerable<AdditionalInfo> Appendix { get; set; }
        public bool IsApproved { get; set; }
        public DateTime ReportTime { get; set; }
        public string ProjectId { get; set; }
        public string ReporterName { get; set; }
        public string TaskId { get; set; }
        public ReportResponse Response { get; set; }
    }
}
