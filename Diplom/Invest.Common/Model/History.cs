using System;

namespace Invest.Common.Model
{
    public class History
    {
        public string Editor { get; set; }
        public string FromState { get; set; }
        public string ToState { get; set; }
        public DateTime EditingTime { get; set; }
    }
}
