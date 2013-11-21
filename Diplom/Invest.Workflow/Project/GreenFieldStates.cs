using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Invest.Workflow.Project
{
    public static class GreenFieldStates
    {
        public const string Open = "Open";

        public const string Progress = "Progress";

        public const string Close = "Close";

        public const string VerifyResponse = "VerifyResponse";

        public const string PendingPlanChanged = "PendingPlanChanged";

        public const string WaitForVerifyResponse = "WaitForVerifyResponse";

        public const string WaitForAssignee = "WaitForAssignee";

        public const string WaitForPlan = "WaitForPlan";
    }
}
