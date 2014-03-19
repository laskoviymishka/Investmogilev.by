// // -----------------------------------------------------------------------
// // <copyright file="DateTimeBinder.cs" author="Andrei Tserakhau">
// // Copyright (c) Andrei Tserakhau. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------

namespace Investmogilev.UI.Portal
{
	#region Using

	using System;
	using System.Globalization;
	using System.Web.Mvc;

	#endregion

	public class DateTimeBinder : IModelBinder
	{
		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).RawValue as string[];
			DateTime date;
			if (!DateTime.TryParse(value[0], out date))
			{
				if (!DateTime.TryParseExact(value[0], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
				{
					throw new ArgumentException("Cannot parse datetime string");
				}
			}

			return date;
		}
	}
}