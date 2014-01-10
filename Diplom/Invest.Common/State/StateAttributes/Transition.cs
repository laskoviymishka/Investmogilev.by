using System;

namespace Invest.Common.State.StateAttributes
{
    public class Transition
    {
        public Transition(string trigger, string destination, string from, string to) { }

        public string From { get; set; }
        public string To { get; set; }
        public string Trigger { get; set; }
        public string DestinationState { get; set; }
        public Func<bool> Guard { get; set; } 
    }
}