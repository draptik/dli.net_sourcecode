using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Autofac;
using System.Web.Routing;

namespace Ploeh.Samples.Commerce.Web.Autofac
{
    public class AutofacControllerFactory : DefaultControllerFactory
    {
        private readonly IContainer container;
        private readonly Dictionary<IController, ILifetimeScope> scopes;
        private readonly object syncRoot;

        public AutofacControllerFactory(IContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            
            this.container = container;
            this.scopes = new Dictionary<IController, ILifetimeScope>();
            this.syncRoot = new object();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            var scope = this.container.BeginLifetimeScope();
            var controller = (IController)scope.Resolve(controllerType);
            lock (this.syncRoot)
            {
                this.scopes.Add(controller, scope);
            }
            return controller;
        }

        public override void ReleaseController(IController controller)
        {
            lock (this.syncRoot)
            {
                var scope = this.scopes[controller];
                this.scopes.Remove(controller);

                scope.Dispose();
            }
            base.ReleaseController(controller);
        }
    }
}
