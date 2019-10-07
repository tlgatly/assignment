using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CSSAssignment
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Assignment",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Assignment", action = "Create", id = UrlParameter.Optional }
            );
        }
    }
}
