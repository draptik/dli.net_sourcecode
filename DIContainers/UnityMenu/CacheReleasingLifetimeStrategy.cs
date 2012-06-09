using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ObjectBuilder2;

namespace Ploeh.Samples.Menu.Unity
{
    public class CacheReleasingLifetimeStrategy : BuilderStrategy
    {
        public override void PostTearDown(
            IBuilderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var lifetimes = context
                .Lifetime.OfType<CacheLifetimeManager>();
            foreach (var lifetimePolicy in lifetimes)
            {
                lifetimePolicy.RemoveValue();
            }
        }
    }
}
