using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InvestPortal.App_Start
{
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