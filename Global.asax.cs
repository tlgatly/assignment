using CSSAssignment.App_Start;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CSSAssignment
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);            

            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                ContractResolver = new DefaultContractResolver()
            };

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings = settings;

            log4net.Config.XmlConfigurator.Configure();
        }
    }
}
