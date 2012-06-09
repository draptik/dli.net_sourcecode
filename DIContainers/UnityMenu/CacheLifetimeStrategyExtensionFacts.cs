using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Microsoft.Practices.Unity;
using Moq;
using Microsoft.Practices.Unity.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder2;

namespace Ploeh.Samples.Menu.Unity
{
    public class CacheLifetimeStrategyExtensionFacts
    {
        [Fact]
        public void SutIsContainerExtension()
        {
            // Fixture setup
            // Exercise system
            var sut = new CacheLifetimeStrategyExtension();
            // Verify outcome
            Assert.IsAssignableFrom<UnityContainerExtension>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeAddsCacheStrategyToContext()
        {
            // Fixture setup
            var strategies = new StagedStrategyChain<UnityBuildStage>();

            var contextStub = new Mock<ExtensionContext> { CallBase = true };
            contextStub.SetupGet(ctx => ctx.Strategies).Returns(strategies);

            var sut = new CacheLifetimeStrategyExtension();
            // Exercise system
            sut.InitializeExtension(contextStub.Object);
            // Verify outcome
            Assert.True(strategies.MakeStrategyChain().OfType<CacheLifetimeStrategy>().Any());
            // Teardown
        }

        [Fact]
        public void InitializeAddsReleasingStrategyToContext()
        {
            // Fixture setup
            var strategies = new StagedStrategyChain<UnityBuildStage>();

            var contextStub = new Mock<ExtensionContext> { CallBase = true };
            contextStub.SetupGet(ctx => ctx.Strategies).Returns(strategies);

            var sut = new CacheLifetimeStrategyExtension();
            // Exercise system
            sut.InitializeExtension(contextStub.Object);
            // Verify outcome
            Assert.True(strategies.MakeStrategyChain().OfType<CacheReleasingLifetimeStrategy>().Any());
            // Teardown
        }
    }
}
