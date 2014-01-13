using System;

namespace Invest.Common.State.StateAttributes
{
    public class Transition
    {
        public Transition() { }

        public Transition(string trigger, string destination, string from, string to) { }

        public object From { get; set; }
        public object To { get; set; }
        public object Trigger { get; set; }
        public string DestinationState { get; set; }
        public Func<bool> Guard { get; set; } 
    }
}