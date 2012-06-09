using System;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;

namespace Ploeh.Samples.Commerce.Web.Windsor
{
    public class WindsorControllerFactory : DefaultControllerFactory
    {
        private readonly IWindsorContainer container;

        public WindsorControllerFactory(IWindsorContainer container)
        {
            this.container = container;
        }

        protected override IController GetControllerInstance(
            RequestContext requestContext, Type controllerType)
        {
            return (IController)this.container.Resolve(controllerType);
        }

        public override void ReleaseController(IController controller)
        {
            this.container.Release(controller);
        }
    }
}
