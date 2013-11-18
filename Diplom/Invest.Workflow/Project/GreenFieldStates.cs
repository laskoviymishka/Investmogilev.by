using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Invest.Workflow.Project
{
    public class GreenFieldStates : BaseProjectStates
    {
        public const string VerifyResponse = "VerifyResponse";

        public const string PendingPlanChanged = "PendingPlanChanged";

        public const string UnVerifyDone = "UnVerifyDone";
    }
}
