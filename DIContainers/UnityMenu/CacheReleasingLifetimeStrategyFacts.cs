using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Microsoft.Practices.ObjectBuilder2;
using Moq;
using Ploeh.Samples.CacheModel;

namespace Ploeh.Samples.Menu.Unity
{
    public class CacheReleasingLifetimeStrategyFacts
    {
        [Fact]
        public void SutIsBuilderStrategy()
        {
            // Fixture setup
            // Exercise system
            var sut = new CacheReleasingLifetimeStrategy();
            // Verify outcome
            Assert.IsAssignableFrom<BuilderStrategy>(sut);
            // Teardown
        }

        [Fact]
        public void PostTearDownWithNullContextThrows()
        {
            // Fixture setup
            var sut = new CacheReleasingLifetimeStrategy();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.PostTearDown(null));
            // Teardown
        }

        [Fact]
        public void PostTearDownRemovesFromAllLifetimePolicies()
        {
            // Fixture setup
            var lifetimeMocks = Enumerable.Range(1, 3).Select(i => new Mock<CacheLifetimeManager>(new Mock<ILease>().Object)).ToList();

            var lifetimeContainer = new LifetimeContainer();
            lifetimeMocks.ForEach(m => lifetimeContainer.Add(m.Object));

            var contextStub = new Mock<IBuilderContext>();
            contextStub.SetupGet(ctx => ctx.Lifetime).Returns(lifetimeContainer);

            var sut = new CacheReleasingLifetimeStrategy();
            // Exercise system
            sut.PostTearDown(contextStub.Object);
            // Verify outcome
            lifetimeMocks.ForEach(m => m.Verify(lp => lp.RemoveValue()));
            // Teardown
        }
    }
}
