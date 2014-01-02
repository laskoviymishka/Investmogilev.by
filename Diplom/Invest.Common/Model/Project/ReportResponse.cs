using System;
using System.Collections.Generic;
using Invest.Common.Model.Common;

namespace Invest.Common.Model.Project
{
    public class ReportResponse: IMongoEntity
    {
        public string Body { get; set; }
        public IEnumerable<AdditionalInfo> Info { get; set; }
        public bool IsApproved { get; set; }
        public DateTime ResponseTime { get; set; }
        public string _id { get; set; }
    }
}
