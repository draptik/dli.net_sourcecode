using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ploeh.Samples.Lifetime.MvcApplication.Controllers;
using System.Web.Routing;

namespace Ploeh.Samples.Lifetime.MvcApplication
{
    public class CommerceControllerFactory : DefaultControllerFactory
    {
        private readonly CommerceContainer container;

        public CommerceControllerFactory(CommerceContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            this.container = container;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == typeof(HomeController))
            {
                return this.container.ResolveHomeController();
            }

            return base.GetControllerInstance(requestContext, controllerType);
        }
    }
}
