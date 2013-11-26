using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;

namespace InvestPortal.Controllers
{
    public class ProfileController : Controller
    {
        [PopulateSiteMap(SiteMapName = "Invest", ViewDataKey = "Invest")]
        public ActionResult ProfilePartial()
        {
            if (!SiteMapManager.SiteMaps.ContainsKey("Invest"))
            {
                SiteMapManager.SiteMaps.Register<XmlSiteMap>("Invest", sitmap => sitmap.LoadFrom("~/App_Data/SiteMap.xml"));
            }

            return PartialView();
        }

    }
}
