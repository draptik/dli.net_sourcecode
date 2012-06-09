using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Objects.Factory;

namespace Ploeh.Samples.Menu.SpringNet
{
    public static class Resolver
    {
        public static T Resolve<T>(this IListableObjectFactory factory)
        {
            return factory.GetObjectsOfType(typeof(T))
                .Values.OfType<T>().Single();
        }
    }
}
