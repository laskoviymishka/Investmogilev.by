using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanelUI.Models
{
	public class RegionParametr
	{
		public string ParametrName { get; set; }
		public string RegionName { get; set; }
		public IEnumerable<YearViewModel> YearDatas { get; set; }

		public CurrentViewModel CurrentData { get; set; }
	}
}