using System;
using System.Collections.Generic;
using Invest.Common.Model.Common;

namespace Invest.Common.Model.Project
{
    public class Report : IMongoEntity
    {
        public string Body { get; set; }
        public string ProjectId { get; set; }
        public string TaskId { get; set; }
        public List<AdditionalInfo> Info { get; set; }
        public ReportResponse ReportResponse { get; set; }
        public DateTime ReportTime { get; set; }
        public string _id { get; set; }
    }
}
