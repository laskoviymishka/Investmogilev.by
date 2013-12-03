using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Invest.Common.Model.Common;

namespace Invest.Common.Model.ProjectModels
{
    public class ReportResponse : IMongoEntity
    {
        public string _id { get; set; }
        public string TaskId { get; set; }
        public string ReportId { get; set; }
        public string ResponseUser { get; set; }
        public string ResponseBody { get; set; }
        public IEnumerable<AdditionalInfo> Appendix { get; set; }
        public DateTime ResponseTime { get; set; }
        public bool IsApproved { get; set; }
        public bool IsReject { get; set; }
    }
}
