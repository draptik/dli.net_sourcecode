using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ObjectBuilder2;

namespace Ploeh.Samples.Menu.Unity
{
    public class CacheLifetimeStrategy : BuilderStrategy
    {
        public override void PreBuildUp(
            IBuilderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            IPolicyList policySource;
            var lifetimePolicy = context
                .PersistentPolicies
                .Get<ILifetimePolicy>(context.BuildKey,
                    out policySource);

            if (object.ReferenceEquals(policySource,
                context.PersistentPolicies))
            {
                return;
            }

            var cacheLifetime =
                lifetimePolicy as CacheLifetimeManager;
            if (cacheLifetime == null)
            {
                return;
            }

            var childLifetime = cacheLifetime.Clone();

            context
                .PersistentPolicies
                .Set<ILifetimePolicy>(childLifetime,
                    context.BuildKey);
            context.Lifetime.Add(childLifetime);
        }
    }
}
