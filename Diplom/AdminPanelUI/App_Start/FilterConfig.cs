using System.Web.Http;
using System.Web.Mvc;
using Westwind.Web.WebApi;

// ReSharper disable CheckNamespace
namespace AdminPanelUI
// ReSharper restore CheckNamespace
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.JsonFormatter);

            GlobalConfiguration.Configuration.Formatters.Add(new JsonNetFormatter());
        }
    }
}