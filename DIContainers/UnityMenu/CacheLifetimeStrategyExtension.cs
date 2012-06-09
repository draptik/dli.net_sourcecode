using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;

namespace Ploeh.Samples.Menu.Unity
{
    public class CacheLifetimeStrategyExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Context.Strategies
                .AddNew<CacheLifetimeStrategy>(
                    UnityBuildStage.Lifetime);
            this.Context.Strategies
                .AddNew<CacheReleasingLifetimeStrategy>(
                    UnityBuildStage.Lifetime);
        }
    }
}
