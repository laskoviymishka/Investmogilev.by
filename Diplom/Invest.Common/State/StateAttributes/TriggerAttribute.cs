using System;

namespace Invest.Common.State.StateAttributes
{
    public class TriggerAttribute : Attribute
    {
        public TriggerAttribute(string workflowName, string triggerName, string from, string to)
        {
            WorkflowName = workflowName;
            TriggerName = triggerName;
            From = from;
            To = to;
        }

        public string WorkflowName { get; private set; }

        public string TriggerName { get; private set; }

        public string From { get; private set; }

        public string To { get; private set; }
    }
}