using System;
using System.Web.Mvc;
using System.Web.Routing;
using StructureMap;

namespace Ploeh.Samples.Commerce.Web.StructureMap
{
    public class StructureMapControllerFactory : DefaultControllerFactory
    {
        private readonly IContainer container;

        public StructureMapControllerFactory(IContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            this.container = container;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return (IController)this.container.GetInstance(controllerType);
        }
    }
}