namespace Investmogilev.UI.Portal.Models
{
	#region Using

	using System.Collections.Generic;

	#endregion

	public class ParametrViewModel
	{
		public string Name { get; set; }
		public string ParentParametrName { get; set; }
		public Dictionary<int, double> Data { get; set; }
		public Dictionary<int, double> Growthes { get; set; }
		public double Growth { get; set; }

		public override string ToString()
		{
			return string.Format("{0} - {1} - {2}",Name, ParentParametrName, Growth);
		}
	}
}