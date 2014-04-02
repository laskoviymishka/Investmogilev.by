namespace Investmogilev.UI.Portal.Models
{
	#region Using

	using System.Collections.Generic;

	#endregion

	public class PartialTableViewModel
	{
		public List<RegionDataViewModel> Regions { get; set; }
		public List<KeyValuePair<string,double>> AveragesTotal { get; set; }
		public List<AvgParametr> AveragesParametrGrowth { get; set; }
	}

	public class AvgParametr
	{
		public string Name { get; set; }
		public Dictionary<int, double> Data { get; set; }
	}
}