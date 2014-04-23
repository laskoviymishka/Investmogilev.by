using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Investmogilev.Infrastructure.Common.Model.Common
{
	public class Activity
	{
		public string Name { get; set; }
		public string RefferenceId { get; set; }
		public string Description { get; set; }
		public ActivityType Type { get; set; }
	}
}
