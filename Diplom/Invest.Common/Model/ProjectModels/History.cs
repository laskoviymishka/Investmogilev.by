using System;
using Invest.Common.State;

namespace Invest.Common.Model.ProjectModels
{
    public class History
    {
        public string Editor { get; set; }
        public ProjectStates FromState { get; set; }
        public ProjectStates ToState { get; set; }
        public DateTime EditingTime { get; set; }
    }
}
