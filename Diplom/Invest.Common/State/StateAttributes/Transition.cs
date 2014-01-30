using System;
using System.Collections.Generic;

namespace Invest.Common.State.StateAttributes
{
	public class Transition
	{
		public Transition() { }

		public Transition(string trigger, string destination, string from, string to) { }

		public object From { get; set; }
		public object To { get; set; }
		public object Trigger { get; set; }
		public Func<bool> Guard { get; set; }
		public override string ToString()
		{
			return string.Format("{0} from {1} to {2}", Trigger, From.ToString(), To.ToString());
		}
	}

	public class TransitionComparer : IEqualityComparer<Transition>
	{

		public bool Equals(Transition x, Transition y)
		{
			return x.ToString() == y.ToString();
		}

		public int GetHashCode(Transition obj)
		{
			return obj.From.GetHashCode() * obj.To.GetHashCode() ^ obj.Trigger.GetHashCode();
		}
	}
}