using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ploeh.Samples.Commerce.Web.Controllers;
using Ploeh.Samples.Commerce.Domain;

namespace Ploeh.Samples.Commerce.Web
{
    public class CommerceControllerFactory : DefaultControllerFactory
    {
        private readonly Dictionary<string, Func<RequestContext, IController>> controllerMap;

        public CommerceControllerFactory(ProductRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }

            this.controllerMap = new Dictionary<string, Func<RequestContext, IController>>();
            this.controllerMap["Account"] = ctx => new AccountController();
            this.controllerMap["Home"] = ctx => new HomeController(repository);
        }

        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            return this.controllerMap[controllerName](requestContext);
        }

        public override void ReleaseController(IController controller)
        {
        }
    }
}
