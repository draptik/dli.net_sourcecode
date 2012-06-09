using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Castle.MicroKernel.Lifestyle;
using Castle.Core;
using Moq;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Ploeh.Samples.CacheModel;
using Castle.MicroKernel.Context;

namespace Ploeh.Samples.Menu.Windsor
{
    public class CacheLifestyleManagerFacts
    {
        [Fact]
        public void SutIsLifestyleManager()
        {
            // Fixture setup
            // Exercise system
            var sut = new CacheLifestyleManager();
            // Verify outcome
            Assert.IsAssignableFrom<AbstractLifestyleManager>(sut);
            // Teardown
        }

        [Fact]
        public void SutHasDefaultConstructor()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.DoesNotThrow(() =>
                new CacheLifestyleManager());
            // Teardown
        }

        [Fact]
        public void LeaseIsCorrectWhenAccessedBeforeInitialization()
        {
            // Fixture setup
            var sut = new CacheLifestyleManager();
            // Exercise system
            ILease result = sut.Lease;
            // Verify outcome
            var actual = Assert.IsAssignableFrom<SlidingLease>(result);
            Assert.Equal(TimeSpan.FromMinutes(1), actual.Timeout);
            // Teardown
        }

        [Fact]
        public void LeaseIsCorrectWhenAccessedAfterInitializationButNoLeaseIsAvailableInKernel()
        {
            // Fixture setup
            var kernel = new DefaultKernel();
            var model = new ComponentModel("foo", typeof(ICloneable), typeof(Version));
            var activator = kernel.CreateComponentActivator(model);

            var sut = new CacheLifestyleManager();
            sut.Init(activator, kernel, model);
            // Exercise system
            var result = sut.Lease;
            // Verify outcome
            var actual = Assert.IsAssignableFrom<SlidingLease>(result);
            Assert.Equal(TimeSpan.FromMinutes(1), actual.Timeout);
            // Teardown
        }

        [Fact]
        public void LeaseIsCorrectWhenAccessedAfterInitializationWhenLeaseIsAvailabeInKernel()
        {
            // Fixture setup
            var kernel = new DefaultKernel();
            var model = new ComponentModel("foo", typeof(ICloneable), typeof(Version));
            var activator = kernel.CreateComponentActivator(model);

            var expectedLease = new Mock<ILease> { DefaultValue = DefaultValue.Mock }.Object;
            kernel.Register(Component.For<ILease>().Instance(expectedLease));

            var sut = new CacheLifestyleManager();
            sut.Init(activator, kernel, model);
            // Exercise system
            var result = sut.Lease;
            // Verify outcome
            Assert.Equal(expectedLease, result);
            // Teardown
        }

        [Fact]
        public void ResolveOnceReturnsCorrectResult()
        {
            // Fixture setup
            var kernel = new DefaultKernel();
            var model = new ComponentModel("foo", typeof(ICloneable), typeof(Version));
            var activator = kernel.CreateComponentActivator(model);

            var sut = new CacheLifestyleManager();
            sut.Init(activator, kernel, model);
            // Exercise system
            var result = sut.Resolve(CreationContext.Empty);
            // Verify outcome
            Assert.IsAssignableFrom<Version>(result);
            // Teardown
        }

        [Fact]
        public void ResolveTwiceReturnsSame()
        {
            // Fixture setup
            var kernel = new DefaultKernel();
            var model = new ComponentModel("foo", typeof(ICloneable), typeof(Version));
            var activator = kernel.CreateComponentActivator(model);

            var sut = new CacheLifestyleManager();
            sut.Init(activator, kernel, model);
            // Exercise system
            var first = sut.Resolve(CreationContext.Empty);
            var second = sut.Resolve(CreationContext.Empty);
            // Verify outcome
            Assert.Same(first, second);
            // Teardown
        }

        [Fact]
        public void FirstResolveWillRenewLease()
        {
            // Fixture setup
            var kernel = new DefaultKernel();
            var model = new ComponentModel("foo", typeof(ICloneable), typeof(Version));
            var activator = kernel.CreateComponentActivator(model);

            var leaseMock = new Mock<ILease> { DefaultValue = DefaultValue.Mock };
            kernel.Register(Component.For<ILease>().Instance(leaseMock.Object));

            var sut = new CacheLifestyleManager();
            sut.Init(activator, kernel, model);
            // Exercise system
            sut.Resolve(CreationContext.Empty);
            // Verify outcome
            leaseMock.Verify(l => l.Renew());
            // Teardown
        }

        [Fact]
        public void MultipleResolvesWillOnlyRenewOnce()
        {
            // Fixture setup
            var kernel = new DefaultKernel();
            var model = new ComponentModel("foo", typeof(ICloneable), typeof(Version));
            var activator = kernel.CreateComponentActivator(model);

            var leaseMock = new Mock<ILease> { DefaultValue = DefaultValue.Mock };
            kernel.Register(Component.For<ILease>().Instance(leaseMock.Object));

            var sut = new CacheLifestyleManager();
            sut.Init(activator, kernel, model);
            // Exercise system
            sut.Resolve(CreationContext.Empty);
            sut.Resolve(CreationContext.Empty);
            // Verify outcome
            leaseMock.Verify(l => l.Renew(), Times.Once());
            // Teardown
        }

        [Fact]
        public void ResolveWillReturnNewInstanceWhenLeaseExpires()
        {
            // Fixture setup
            var kernel = new DefaultKernel();
            var model = new ComponentModel("foo", typeof(ICloneable), typeof(Version));
            var activator = kernel.CreateComponentActivator(model);

            var expired = false;
            var leaseStub = new Mock<ILease> { DefaultValue = DefaultValue.Mock };
            leaseStub.Setup(l => l.IsExpired).Returns(() =>
                {
                    var b = expired;
                    expired = !expired;
                    return b;
                });
            kernel.Register(Component.For<ILease>().Instance(leaseStub.Object));

            var sut = new CacheLifestyleManager();
            sut.Init(activator, kernel, model);
            // Exercise system
            var first = sut.Resolve(CreationContext.Empty);
            var second = sut.Resolve(CreationContext.Empty);
            // Verify outcome
            Assert.NotSame(first, second);
            // Teardown
        }

        [Fact]
        public void ReleaseAndResolveWillReturnCachedInstance()
        {
            // Fixture setup
            var kernel = new DefaultKernel();
            var model = new ComponentModel("foo", typeof(ICloneable), typeof(Version));
            var activator = kernel.CreateComponentActivator(model);

            var leaseMock = new Mock<ILease> { DefaultValue = DefaultValue.Mock };
            kernel.Register(Component.For<ILease>().Instance(leaseMock.Object));

            var sut = new CacheLifestyleManager();
            sut.Init(activator, kernel, model);

            var first = sut.Resolve(CreationContext.Empty);
            // Exercise system
            sut.Release(first);
            // Verify outcome
            var second = sut.Resolve(CreationContext.Empty);
            Assert.Same(first, second);
            // Teardown
        }

        [Fact]
        public void ReleaseWrongObjectWillNotReleaseInstance()
        {
            // Fixture setup
            var kernel = new DefaultKernel();
            var model = new ComponentModel("foo", typeof(ICloneable), typeof(Version));
            var activator = kernel.CreateComponentActivator(model);

            var leaseMock = new Mock<ILease> { DefaultValue = DefaultValue.Mock };
            kernel.Register(Component.For<ILease>().Instance(leaseMock.Object));

            var sut = new CacheLifestyleManager();
            sut.Init(activator, kernel, model);

            var first = sut.Resolve(CreationContext.Empty);
            // Exercise system
            sut.Release(new object());
            // Verify outcome
            var second = sut.Resolve(CreationContext.Empty);
            Assert.Same(first, second);
            // Teardown
        }
    }
}
