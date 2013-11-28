namespace BusinessLogic.Managers
{
    public static class GreenFieldStates
    {
        public const string Empty = "Empty";

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
