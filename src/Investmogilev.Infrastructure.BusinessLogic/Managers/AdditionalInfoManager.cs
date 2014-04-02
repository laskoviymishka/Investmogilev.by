// // -----------------------------------------------------------------------
// // <copyright file="AdditionalInfoManager.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.Infrastructure.BusinessLogic.Managers
{
	#region Using

	using System;
	using System.IO;

	#endregion

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