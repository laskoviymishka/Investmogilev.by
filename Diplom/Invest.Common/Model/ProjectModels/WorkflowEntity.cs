using System.Collections.Generic;

namespace Invest.Common.Model.ProjectModels
{
    public class WorkflowEntity 
    {
        public string CurrenState { get; set; }
        public IList<History> ChangeHistory { get; set; }


        public override string ToString()
        {
            return CurrenState;
        }
    }
}