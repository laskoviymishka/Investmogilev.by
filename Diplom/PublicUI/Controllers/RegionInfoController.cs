using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace PublicUI.Controllers
{
    public class RegionInfoController : Controller
    {
        public ActionResult GetRegionInfo(string name)
        {
            Response.AddHeader("Access-Control-Allow-Origin", "*");
            ViewBag.Name = name;
            return PartialView();
        }
    }
}
