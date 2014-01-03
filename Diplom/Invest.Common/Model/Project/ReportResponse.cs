using System;
using System.Collections.Generic;
using Invest.Common.Model.Common;

namespace Invest.Common.Model.Project
{
    public class ReportResponse: IMongoEntity
    {
        public string Body { get; set; }
        public List<AdditionalInfo> Info { get; set; }
        public bool IsApproved { get; set; }
        public DateTime ResponseTime { get; set; }
        public string _id { get; set; }
        public string ProjectId { get; set; }
        public string ReportId { get; set; }
        public string TaskId { get; set; }
        public string ResponseUser { get; set; }
    }
}
