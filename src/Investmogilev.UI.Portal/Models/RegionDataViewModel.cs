namespace Investmogilev.UI.Portal.Models
{
	#region Using

	using System.Collections.Generic;

	#endregion

	public class RegionDataViewModel
	{
		public string RegionName { get; set; }
		public List<ParametrViewModel> Parametrs { get; set; }

		public override string ToString()
		{
			return RegionName + "  " + Parametrs.Count;
		}
	}
}