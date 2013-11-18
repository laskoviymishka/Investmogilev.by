using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Invest.Workflow.StateManagment
{
    public class History
    {
        public string Editor { get; set; }
        public string FromState { get; set; }
        public string ToState { get; set; }
        public DateTime EditingTime { get; set; }
    }
}
