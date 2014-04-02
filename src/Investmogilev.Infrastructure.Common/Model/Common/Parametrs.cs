// // -----------------------------------------------------------------------
// // <copyright file="Parametrs.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.Common.Model.Common
{
	#region Using

	using System.Collections.Generic;

	#endregion

	public class Parametrs
	{
		public string ParametrName { get; set; }

		public double IntegralValue { get; set; }

		public List<KeyValuePair<int, double>> Values { get; set; }

		public List<KeyValuePair<int, double>> AbsoluteValues { get; set; }

		public List<KeyValuePair<int, double>> DependValues { get; set; }

		public List<KeyValuePair<int, double>> DependAbsoluteValues { get; set; }

		public IList<Parametrs> ChildParametrs { get; set; }
	}
}