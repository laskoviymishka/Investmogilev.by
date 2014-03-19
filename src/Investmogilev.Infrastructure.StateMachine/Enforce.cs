// // -----------------------------------------------------------------------
// // <copyright file="Enforce.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.StateMachine
{
	#region Using

	using System;

	#endregion

	internal static class Enforce
	{
		public static T ArgumentNotNull<T>(T argument, string description)
			where T : class
		{
			if (argument == null)
				throw new ArgumentNullException(description);

			return argument;
		}
	}
}