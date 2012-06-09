using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using Ploeh.Samples.Commerce.Domain;

namespace Ploeh.Samples.Commerce.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            MvcApplication.RegisterRoutes(RouteTable.Routes);

            var container = 
                new WindsorContainer(new XmlInterpreter());
            var repository =
                container.Resolve<ProductRepository>();
            var controllerFactory = 
                new CommerceControllerFactory(repository);

            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }
    }
}