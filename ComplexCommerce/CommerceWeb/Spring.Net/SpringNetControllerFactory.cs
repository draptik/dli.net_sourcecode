using System;
using System.Collections;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Spring.Objects.Factory;

namespace Ploeh.Samples.Commerce.Web.Spring.Net
{
    public class SpringNetControllerFactory : DefaultControllerFactory
    {
        private readonly IListableObjectFactory context;

        public SpringNetControllerFactory(IListableObjectFactory context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            this.context = context;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            IDictionary controllers =
                this.context.GetObjectsOfType(controllerType);
            return controllers.Values.OfType<IController>().Single();
        }
    }
}
