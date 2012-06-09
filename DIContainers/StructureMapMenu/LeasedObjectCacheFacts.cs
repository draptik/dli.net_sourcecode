using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Moq;
using StructureMap.Pipeline;
using Xunit.Extensions;
using Ploeh.Samples.CacheModel;

namespace Ploeh.Samples.Menu.StructureMap
{
    public class LeasedObjectCacheFacts
    {
        [Fact]
        public void SutIsObjectCache()
        {
            // Fixture setup
            var dummyLease = new Mock<ILease>().Object;
            // Exercise system
            var sut = new LeasedObjectCache(dummyLease);
            // Verify outcome
            Assert.IsAssignableFrom<IObjectCache>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullLeaseThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new LeasedObjectCache(null));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullLeaseAndObjectCacheThrows()
        {
            // Fixture setup
            var dummyCache = new Mock<IObjectCache>().Object;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new LeasedObjectCache(null, dummyCache));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullObjectCacheThrows()
        {
            // Fixture setup
            var dummyLease = new Mock<ILease>().Object;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new LeasedObjectCache(dummyLease, null));
            // Teardown
        }

        [Fact]
        public void LeaseIsCorrectWhenInitializedWithMinimalConstructor()
        {
            // Fixture setup
            var expectedLease = new Mock<ILease>().Object;
            var sut = new LeasedObjectCache(expectedLease);
            // Exercise system
            ILease result = sut.Lease;
            // Verify outcome
            Assert.Equal(expectedLease, result);
            // Teardown
        }

        [Fact]
        public void LeaseIsCorrectWhenInitializedWithGreedyConstructor()
        {
            // Fixture setup
            var expectedLease = new Mock<ILease>().Object;
            var dummyCache = new Mock<IObjectCache>().Object;
            var sut = new LeasedObjectCache(expectedLease, dummyCache);
            // Exercise system
            var result = sut.Lease;
            // Verify outcome
            Assert.Equal(expectedLease, result);
            // Teardown
        }

        [Fact]
        public void ObjectCacheIsCorrectWhenInitializedWithMinimalConstructor()
        {
            // Fixture setup
            var dummyLease = new Mock<ILease>().Object;
            var sut = new LeasedObjectCache(dummyLease);
            // Exercise system
            IObjectCache result = sut.ObjectCache;
            // Verify outcome
            Assert.IsAssignableFrom<MainObjectCache>(result);
            // Teardown
        }

        [Fact]
        public void ObjectCacheIsCorrectWhenInitializedWithGreedyConstructor()
        {
            // Fixture setup
            var dummyLease = new Mock<ILease>().Object;
            var expectedCache = new Mock<IObjectCache>().Object;
            var sut = new LeasedObjectCache(dummyLease, expectedCache);
            // Exercise system
            var result = sut.ObjectCache;
            // Verify outcome
            Assert.Equal(expectedCache, result);
            // Teardown
        }

        [Fact]
        public void CountReturnsCorrectResult()
        {
            // Fixture setup
            var expectedCount = 9;

            var dummyLease = new Mock<ILease>().Object;

            var cacheStub = new Mock<IObjectCache>();
            cacheStub.SetupGet(c => c.Count).Returns(expectedCount);

            var sut = new LeasedObjectCache(dummyLease, cacheStub.Object);
            // Exercise system
            var result = sut.Count;
            // Verify outcome
            Assert.Equal(expectedCount, result);
            // Teardown
        }

        [Theory]
        [InlineData(true, 1)]
        [InlineData(false, 0)]
        public void CountClearsCacheAppropriately(bool isExpired, int clearCount)
        {
            // Fixture setup
            var leaseStub = new Mock<ILease>();
            leaseStub.SetupGet(l => l.IsExpired).Returns(isExpired);

            var cacheMock = new Mock<IObjectCache>();

            var sut = new LeasedObjectCache(leaseStub.Object, cacheMock.Object);
            // Exercise system
            var ignored = sut.Count;
            // Verify outcome
            cacheMock.Verify(l => l.DisposeAndClear(), Times.Exactly(clearCount));
            // Teardown
        }

        [Fact]
        public void DisposeAndClearsDisposesAndClearsDecoratedCache()
        {
            // Fixture setup
            var dummyLease = new Mock<ILease>().Object;

            var cacheMock = new Mock<IObjectCache>();

            var sut = new LeasedObjectCache(dummyLease, cacheMock.Object);
            // Exercise system
            sut.DisposeAndClear();
            // Verify outcome
            cacheMock.Verify(c => c.DisposeAndClear());
            // Teardown
        }

        [Fact]
        public void EjectEjectsFromDecoratedCache()
        {
            // Fixture setup
            var dummyLease = new Mock<ILease>().Object;

            var cacheMock = new Mock<IObjectCache>();

            var sut = new LeasedObjectCache(dummyLease, cacheMock.Object);

            var anonymousType = typeof(object);
            var anonymousInstance = new ConstructorInstance(anonymousType);
            // Exercise system
            sut.Eject(anonymousType, anonymousInstance);
            // Verify outcome
            cacheMock.Verify(c => c.Eject(anonymousType, anonymousInstance));
            // Teardown
        }

        [Theory]
        [InlineData(true, 1)]
        [InlineData(false, 0)]
        public void EjectClearsCacheAppropriately(bool isExpired, int clearCount)
        {
            // Fixture setup
            var leaseStub = new Mock<ILease>();
            leaseStub.SetupGet(l => l.IsExpired).Returns(isExpired);

            var cacheMock = new Mock<IObjectCache>();

            var sut = new LeasedObjectCache(leaseStub.Object, cacheMock.Object);
            // Exercise system
            var dummyType = typeof(object);
            var dummyInstance = new ConstructorInstance(dummyType);
            sut.Eject(dummyType, dummyInstance);
            // Verify outcome
            cacheMock.Verify(c => c.DisposeAndClear(), Times.Exactly(clearCount));
            // Teardown
        }

        [Fact]
        public void GetReturnsCorrectResult()
        {
            // Fixture setup
            var dummyLease = new Mock<ILease>().Object;

            var anonymousType = typeof(object);
            var anonymousInstance = new ConstructorInstance(anonymousType);
            var expected = new object();
            var cacheStub = new Mock<IObjectCache>();
            cacheStub.Setup(c => c.Get(anonymousType, anonymousInstance)).Returns(expected);

            var sut = new LeasedObjectCache(dummyLease, cacheStub.Object);
            // Exercise system
            var result = sut.Get(anonymousType, anonymousInstance);
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }

        [Theory]
        [InlineData(true, 1)]
        [InlineData(false, 0)]
        public void GetClearsCacheAppropriately(bool isExpired, int clearCount)
        {
            // Fixture setup
            var leaseStub = new Mock<ILease>();
            leaseStub.SetupGet(l => l.IsExpired).Returns(isExpired);

            var cacheMock = new Mock<IObjectCache>();

            var sut = new LeasedObjectCache(leaseStub.Object, cacheMock.Object);
            // Exercise system
            var dummyType = typeof(object);
            var dummyInstance = new ConstructorInstance(dummyType);
            sut.Get(dummyType, dummyInstance);
            // Verify outcome
            cacheMock.Verify(c => c.DisposeAndClear(), Times.Exactly(clearCount));
            // Teardown
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void HasReturnsCorrectResult(bool expected)
        {
            // Fixture setup
            var dummyLease = new Mock<ILease>().Object;

            var anonymousType = typeof(object);
            var anonymousInstance = new ConstructorInstance(anonymousType);
            var cacheStub = new Mock<IObjectCache>();
            cacheStub.Setup(c => c.Has(anonymousType, anonymousInstance)).Returns(expected);

            var sut = new LeasedObjectCache(dummyLease, cacheStub.Object);
            // Exercise system
            var result = sut.Has(anonymousType, anonymousInstance);
            // Verify outcome
            Assert.Equal(expected, result);
            // Teardown
        }

        [Theory]
        [InlineData(true, 1)]
        [InlineData(false, 0)]
        public void HasClearsCacheAppropriately(bool isExpired, int clearCount)
        {
            // Fixture setup
            var leaseStub = new Mock<ILease>();
            leaseStub.SetupGet(l => l.IsExpired).Returns(isExpired);

            var cacheMock = new Mock<IObjectCache>();

            var sut = new LeasedObjectCache(leaseStub.Object, cacheMock.Object);
            // Exercise system
            var dummyType = typeof(object);
            var dummyInstance = new ConstructorInstance(dummyType);
            sut.Has(dummyType, dummyInstance);
            // Verify outcome
            cacheMock.Verify(c => c.DisposeAndClear(), Times.Exactly(clearCount));
            // Teardown
        }

        [Fact]
        public void LockerIsInstance()
        {
            // Fixture setup
            var dummyLease = new Mock<ILease>().Object;
            var dummyCache = new Mock<IObjectCache> { DefaultValue = DefaultValue.Mock }.Object;
            var sut = new LeasedObjectCache(dummyLease, dummyCache);
            // Exercise system
            var result = sut.Locker;
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void LockerIsStable()
        {
            // Fixture setup
            var dummyLease = new Mock<ILease>().Object;
            var dummyCache = new Mock<IObjectCache>().Object;
            var sut = new LeasedObjectCache(dummyLease, dummyCache);
            var expected = sut.Locker;
            // Exercise system
            var result = sut.Locker;
            // Verify outcome
            Assert.Same(expected, result);
            // Teardown
        }

        [Fact]
        public void SetSetsOnDecoratedCache()
        {
            // Fixture setup
            var anonymousType = typeof(object);
            var anonymousInstance = new ConstructorInstance(anonymousType);
            var anonymousValue = new object();

            var dummyLease = new Mock<ILease>().Object;
            var cacheMock = new Mock<IObjectCache>();

            var sut = new LeasedObjectCache(dummyLease, cacheMock.Object);
            // Exercise system
            sut.Set(anonymousType, anonymousInstance, anonymousValue);
            // Verify outcome
            cacheMock.Verify(c => c.Set(anonymousType, anonymousInstance, anonymousValue));
            // Teardown
        }

        [Fact]
        public void SetRenewsLease()
        {
            // Fixture setup
            var leaseMock = new Mock<ILease>();
            var dummyCache = new Mock<IObjectCache>().Object;

            var sut = new LeasedObjectCache(leaseMock.Object, dummyCache);
            // Exercise system
            var anonymousType = typeof(object);
            var anonymousInstance = new ConstructorInstance(anonymousType);
            var anonymousValue = new object();
            sut.Set(anonymousType, anonymousInstance, anonymousValue);
            // Verify outcome
            leaseMock.Verify(l => l.Renew());
            // Teardown
        }
    }
}
