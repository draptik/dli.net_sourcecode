using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.DI.AntiPatterns.ServiceLocator
{
    public static class Locator
    {
        private readonly static Dictionary<Type, object> services
            = new Dictionary<Type, object>();

        public static T GetService<T>()
        {
            return (T)Locator.services[typeof(T)];
        }

        public static void Register<T>(T service)
        {
            Locator.services[typeof(T)] = service;
        }

        public static void Reset()
        {
            Locator.services.Clear();
        }
    }
}
