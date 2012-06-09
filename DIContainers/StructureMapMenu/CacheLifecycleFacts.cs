using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Moq;
using StructureMap.Pipeline;
using Ploeh.Samples.CacheModel;

namespace Ploeh.Samples.Menu.StructureMap
{
    public class CacheLifecycleFacts
    {
        [Fact]
        public void SutIsLifecycle()
        {
            // Fixture setup
            var dummyLease = new Mock<ILease>().Object;
            // Exercise system
            var sut = new CacheLifecycle(dummyLease);
            // Verify outcome
            Assert.IsAssignableFrom<ILifecycle>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullLeaseThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new CacheLifecycle(null));
            // Teardown
        }

        [Fact]
        public void LeaseIsCorrect()
        {
            // Fixture setup
            var expectedLease = new Mock<ILease>().Object;
            var sut = new CacheLifecycle(expectedLease);
            // Exercise system
            ILease result = sut.Lease;
            // Verify outcome
            Assert.Equal(expectedLease, result);
            // Teardown
        }

        [Fact]
        public void ScopeIsCorrect()
        {
            // Fixture setup
            var dummyLease = new Mock<ILease>().Object;
            var sut = new CacheLifecycle(dummyLease);
            // Exercise system
            var result = sut.Scope;
            // Verify outcome
            Assert.Equal("Cache", result);
            // Teardown
        }

        [Fact]
        public void FindCacheReturnsCorrectResult()
        {
            // Fixture setup
            var expectedLease = new Mock<ILease>().Object;
            var sut = new CacheLifecycle(expectedLease);
            // Exercise system
            var result = sut.FindCache();
            // Verify outcome
            var cache = Assert.IsAssignableFrom<LeasedObjectCache>(result);
            Assert.Equal(expectedLease, cache.Lease);
            // Teardown
        }

        [Fact]
        public void FindCacheReturnsSameResultEveryTime()
        {
            // Fixture setup
            var dummyLease = new Mock<ILease>().Object;
            var sut = new CacheLifecycle(dummyLease);
            var expectedCache = sut.FindCache();
            // Exercise system
            var result = sut.FindCache();
            // Verify outcome
            Assert.Same(expectedCache, result);
            // Teardown
        }
    }
}
