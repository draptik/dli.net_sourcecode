using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using Ploeh.Samples.Commerce.Data.Sql;
using Ploeh.Samples.Commerce.Domain;
using Ploeh.Samples.Commerce.Web.PresentationModel;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using Castle.MicroKernel.Registration;
using Ploeh.Samples.Commerce.Web.PresentationModel.Controllers;
using Ploeh.Samples.Commerce.Web.Windsor;
using StructureMap;
using Autofac;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.ComponentModel.Composition.Hosting;
using Spring.Context.Support;

namespace Ploeh.Samples.Commerce.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("favicon.ico");

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );

        }

        #region Poor Man's DI
        protected void Application_Start()
        {
            MvcApplication.RegisterRoutes(RouteTable.Routes);

            var controllerFactory = new CommerceControllerFactory();

            ControllerBuilder.Current.SetControllerFactory(
                controllerFactory);
        }
        #endregion

        #region Castle Windsor, based on web.config
        /* Uncomment this section and comment out the current Application_Start to use this
         * configuration */
        //protected void Application_Start()
        //{
        //    MvcApplication.RegisterRoutes(RouteTable.Routes);

        //    var container =
        //        new WindsorContainer(new XmlInterpreter());
        //    var controllerFactory =
        //        container.Resolve<IControllerFactory>();

        //    ControllerBuilder.Current.SetControllerFactory(
        //        controllerFactory);
        //}
        #endregion

        #region Castle Windsor, convention-based
        /* Uncomment this section and comment out the current Application_Start to use this
         * configuration */
        //protected void Application_Start()
        //{
        //    MvcApplication.RegisterRoutes(RouteTable.Routes);

        //    var container = new WindsorContainer();
        //    container.Install(new CommerceWindsorInstaller());

        //    var controllerFactory =
        //        new WindsorControllerFactory(container);

        //    ControllerBuilder.Current.SetControllerFactory(
        //        controllerFactory);
        //}
        #endregion

        #region StructureMap, Code as Configuration
        /* Uncomment this section and comment out the current Application_Start to use this
         * configuration */
        //protected void Application_Start()
        //{
        //    MvcApplication.RegisterRoutes(RouteTable.Routes);

        //    var container = new Container();
        //    Ploeh.Samples.Commerce.Web.StructureMap.CommerceCodeAsConfiguration.Configure(container);

        //    var controllerFactory =
        //        new Ploeh.Samples.Commerce.Web.StructureMap.StructureMapControllerFactory(container);

        //    ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        //}
        #endregion

        #region StructureMap, convention-based
        /* Uncomment this section and comment out the current Application_Start to use this
         * configuration */
        //protected void Application_Start()
        //{
        //    MvcApplication.RegisterRoutes(RouteTable.Routes);

        //    var container = new Container();

        //    container.Configure(x =>
        //        x.AddRegistry<Ploeh.Samples.Commerce.Web.StructureMap.CommerceRegistry>());

        //    var controllerFactory =
        //        new Ploeh.Samples.Commerce.Web.StructureMap.StructureMapControllerFactory(container);

        //    ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        //}
        #endregion

        #region Spring.NET
        /* Uncomment this section and comment out the current Application_Start to use this
         * configuration */
        //protected void Application_Start()
        //{
        //    MvcApplication.RegisterRoutes(RouteTable.Routes);

        //    var fileName = this.Server.MapPath("springconfig.xml");
        //    var context = new XmlApplicationContext(fileName);

        //    var controllerFactory =
        //        new Spring.Net.SpringNetControllerFactory(context);

        //    ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        //}
        #endregion

        #region autofac, convention-based
        /* Uncomment this section and comment out the current Application_Start to use this
         * configuration */
        //protected void Application_Start()
        //{
        //    MvcApplication.RegisterRoutes(RouteTable.Routes);

        //    var builder = new ContainerBuilder();

        //    builder.RegisterModule<Ploeh.Samples.Commerce.Web.Autofac.CommerceModule>();

        //    var controllerFactory =
        //        new Ploeh.Samples.Commerce.Web.Autofac.AutofacControllerFactory(builder.Build());

        //    ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        //}
        #endregion

        #region Unity, Code as Configuration
        /* Uncomment this section and comment out the current Application_Start to use this
         * configuration */
        //protected void Application_Start()
        //{
        //    MvcApplication.RegisterRoutes(RouteTable.Routes);

        //    var container = new UnityContainer();
        //    container.AddNewExtension<Ploeh.Samples.Commerce.Web.Unity.CommerceContainerExtension>();

        //    var controllerFactory =
        //        new Ploeh.Samples.Commerce.Web.Unity.UnityControllerFactory(container);

        //    ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        //}
        #endregion

        #region Unity, based on web.config
        /* Uncomment this section and comment out the current Application_Start to use this
         * configuration */
        //protected void Application_Start()
        //{
        //    MvcApplication.RegisterRoutes(RouteTable.Routes);

        //    var container = new UnityContainer();
        //    container.LoadConfiguration();

        //    var controllerFactory =
        //        new Ploeh.Samples.Commerce.Web.Unity.UnityControllerFactory(container);

        //    ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        //}

        #endregion

        #region MEF
        /* Uncomment this section and comment out the current Application_Start to use this
         * configuration */
        //protected void Application_Start()
        //{
        //    MvcApplication.RegisterRoutes(RouteTable.Routes);

        //    var catalog = new Ploeh.Samples.Commerce.Web.Mef.CommerceCatalog();
        //    var container = new CompositionContainer(catalog);

        //    var controllerFactory =
        //        new Ploeh.Samples.Commerce.Web.Mef.MefControllerFactory(container);

        //    ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        //}
        #endregion
    }
}