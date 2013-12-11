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

            var date = DateTime.ParseExact(value[0], "dd/MM/yyyy", CultureInfo.InvariantCulture);

            return date;
        }
    }
}