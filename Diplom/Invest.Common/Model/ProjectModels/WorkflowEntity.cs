using System.Collections.Generic;
using Invest.Common.State;

namespace Invest.Common.Model.ProjectModels
{
    public class WorkflowEntity 
    {
        public ProjectStates CurrenState { get; set; }
        public IList<History> ChangeHistory { get; set; }


        public override string ToString()
        {
            return CurrenState.ToString();
        }
    }
}