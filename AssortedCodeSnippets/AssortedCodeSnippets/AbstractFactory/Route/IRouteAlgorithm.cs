using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.AssortedCodeSnippets.AbstractFactory.Route
{
    public interface IRouteAlgorithm
    {
        IRoute CalculateRoute(RouteSpecification specification);
    }
}
