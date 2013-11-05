using System.Web.Mvc;
using System.Web.Routing;

// ReSharper disable CheckNamespace
namespace AdminPanelUI
// ReSharper restore CheckNamespace
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                    "ItemDetailsWithSender",
                    "Region/ChildParametr/{regionId}/{parametrName}",
                    new { controller = "Region", action = "ChildParametr" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}