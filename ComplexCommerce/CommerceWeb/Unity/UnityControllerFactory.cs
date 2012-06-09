using System;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.Unity;

namespace Ploeh.Samples.Commerce.Web.Unity
{
    public class UnityControllerFactory : DefaultControllerFactory
    {
        private readonly IUnityContainer container;

        public UnityControllerFactory(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            this.container = container;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return (IController)this.container.Resolve(controllerType);
        }

        public override void ReleaseController(IController controller)
        {
            // Warning: this doesn't really accomplish what we think it will. See chapter 14 for a
            // full discussion of properly releasing components with Unity.
            this.container.Teardown(controller);
            base.ReleaseController(controller);
        }
    }
}
