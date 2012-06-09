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
    public class CacheLifetimeStrategyFacts
    {
        [Fact]
        public void SutIsBuilderStrategy()
        {
            // Fixture setup
            // Exercise system
            var sut = new CacheLifetimeStrategy();
            // Verify outcome
            Assert.IsAssignableFrom<BuilderStrategy>(sut);
            // Teardown
        }

        [Fact]
        public void PreBuildUpWithNullContextThrows()
        {
            // Fixture setup
            var sut = new CacheLifetimeStrategy();
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.PreBuildUp(null));
            // Teardown
        }

        [Fact]
        public void PreBuildUpWithCacheLifetimeFromParentAddsCorrectPolicyToContext()
        {
            // Fixture setup
            var dummyLease = new Mock<ILease>().Object;
            var originalLifetimeManager = new CacheLifetimeManager(dummyLease);

            var parentList = new Mock<IPolicyList>().Object;

            var buildKey = new NamedTypeBuildKey<Version>();

            var contextMock = new Mock<IBuilderContext> { DefaultValue = DefaultValue.Mock };
            contextMock.SetupGet(ctx => ctx.BuildKey).Returns(buildKey);
            contextMock.Setup(ctx => ctx.PersistentPolicies.Get(typeof(ILifetimePolicy), buildKey, false, out parentList)).Returns(originalLifetimeManager);

            var sut = new CacheLifetimeStrategy();
            // Exercise system
            sut.PreBuildUp(contextMock.Object);
            // Verify outcome
            contextMock.Verify(ctx => ctx.PersistentPolicies.Set(typeof(ILifetimePolicy), It.Is<ILifetimePolicy>(p => new CacheLifetimeManagerCloneLikeness(originalLifetimeManager).Equals(p)), buildKey));
            // Teardown
        }

        [Fact]
        public void PreBuildUpWithCacheLifetimeFromParentAddsCorrectLifetime()
        {
            // Fixture setup
            var dummyLease = new Mock<ILease>().Object;
            var originalLifetimeManager = new CacheLifetimeManager(dummyLease);

            var parentList = new Mock<IPolicyList>().Object;

            var buildKey = new NamedTypeBuildKey<Version>();

            var contextMock = new Mock<IBuilderContext> { DefaultValue = DefaultValue.Mock };
            contextMock.SetupGet(ctx => ctx.BuildKey).Returns(buildKey);
            contextMock.Setup(ctx => ctx.PersistentPolicies.Get(typeof(ILifetimePolicy), buildKey, false, out parentList)).Returns(originalLifetimeManager);

            var sut = new CacheLifetimeStrategy();
            // Exercise system
            sut.PreBuildUp(contextMock.Object);
            // Verify outcome
            contextMock.Verify(ctx => ctx.Lifetime.Add(It.Is<object>(obj => new CacheLifetimeManagerCloneLikeness(originalLifetimeManager).Equals(obj))));
            // Teardown
        }

        private class CacheLifetimeManagerCloneLikeness
        {
            private readonly CacheLifetimeManager target;

            public CacheLifetimeManagerCloneLikeness(CacheLifetimeManager target)
            {
                if (target == null)
                {
                    throw new ArgumentNullException("target");
                }

                this.target = target;
            }

            public override bool Equals(object obj)
            {
                var clm = obj as CacheLifetimeManager;
                if (clm != null)
                {
                    return !this.target.Equals(clm)
                        && clm.Lease.Equals(this.target.Lease);
                }
                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return this.target.Lease.GetHashCode();
            }
        }
    }
}
