using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ex3
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute("display", "display/{ip}/{port}/{time}",
                defaults: new { Controller = "Home", action = "display", time = 0 });

            routes.MapRoute("save", "save/{ip}/{port}/{rate}/{time}/{fileName}",
                defaults: new { Controller = "Home", action = "save" });
        }
    }
}
