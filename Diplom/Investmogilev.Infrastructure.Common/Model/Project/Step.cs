using System;
using System.Collections.Generic;

namespace Investmogilev.Infrastructure.Common.Model.Project
{
    public class Step
    {
        public string Name { get; set; }
        public string Body { get; set; }
        public DateTime Milestone { get; set; }
        public DateTime PassedDate { get; set; }
        public bool IsDone { get; set; }
        public IEnumerable<Report> Reports { get; set; } 
    }
}
