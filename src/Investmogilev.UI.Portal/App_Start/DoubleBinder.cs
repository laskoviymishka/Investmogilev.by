// // -----------------------------------------------------------------------
// // <copyright file="DoubleBinder.cs" author="Andrei Tserakhau">
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

	public class DoubleBinder : IModelBinder
	{
		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
			if (string.IsNullOrEmpty(valueResult.AttemptedValue))
			{
				return 0.0;
			}
			var modelState = new ModelState {Value = valueResult};
			object actualValue = null;
			try
			{
				actualValue = Convert.ToDouble(
					valueResult.AttemptedValue.Replace(",", "."),
					CultureInfo.InvariantCulture
					);
			}
			catch (FormatException e)
			{
				modelState.Errors.Add(e);
			}

			bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
			return actualValue;
		}
	}
}