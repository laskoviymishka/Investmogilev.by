using System;
using System.IO;

namespace Investmogilev.Infrastructure.BusinessLogic.Managers
{
	public class AdditionalInfoManager
	{
		public static string GetPhysicalPath(string template, string[] args)
		{
			string physicalPath = Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
				string.Format(template, args));

			if (!Directory.Exists(physicalPath))
			{
				Directory.CreateDirectory(physicalPath);
			}

			return physicalPath;
		}
	}
}