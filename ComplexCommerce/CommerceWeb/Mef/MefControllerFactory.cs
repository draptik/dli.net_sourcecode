using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.Composition.Hosting;
using System.Web.Mvc;
using System.ComponentModel.Composition.Primitives;
using System.Web.Routing;

namespace Ploeh.Samples.Commerce.Web.Mef
{
    public class MefControllerFactory : DefaultControllerFactory
    {
        private readonly CompositionContainer container;
        private readonly Dictionary<IController, Lazy<object, object>> exports;
        private readonly object syncRoot;

        public MefControllerFactory(CompositionContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            this.container = container;
            this.exports = new Dictionary<IController, Lazy<object, object>>();
            this.syncRoot = new object();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            var export = this.container.GetExports(
                controllerType, null, null).Single();
            var controller = (IController)export.Value;
            lock (this.syncRoot)
            {
                this.exports.Add(controller, export);
            }
            return controller;
        }

        public override void ReleaseController(IController controller)
        {
            lock (this.syncRoot)
            {
                var export = this.exports[controller];
                this.exports.Remove(controller);

                this.container.ReleaseExport(export);
            }
            base.ReleaseController(controller);
        }
    }
}
