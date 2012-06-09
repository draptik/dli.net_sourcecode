using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.AssortedCodeSnippets.AbstractFactory.Route
{
    public class RouteController
    {
        private readonly IRouteAlgorithmFactory factory;

        public RouteController(IRouteAlgorithmFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            this.factory = factory;
        }

        public IRoute GetRoute(RouteSpecification spec,
            RouteType routeType)
        {
            IRouteAlgorithm algorithm = 
                this.factory.CreateAlgorithm(routeType);
            return algorithm.CalculateRoute(spec);
        }
    }
}
