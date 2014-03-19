// // -----------------------------------------------------------------------
// // <copyright file="Transition.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.Common.State.StateAttributes
{
	#region Using

	using System;
	using System.Collections.Generic;

	#endregion

	public class Transition
	{
		public object From { get; set; }
		public object To { get; set; }
		public object Trigger { get; set; }
		public Func<bool> Guard { get; set; }

		public override string ToString()
		{
			return string.Format("{0} from {1} to {2}", Trigger, From, To);
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
			return obj.From.GetHashCode()*obj.To.GetHashCode() ^ obj.Trigger.GetHashCode();
		}
	}
}