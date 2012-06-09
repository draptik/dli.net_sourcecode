using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Moq;
using Ploeh.Samples.CacheModel;
using Microsoft.Practices.Unity;
using Xunit.Extensions;

namespace Ploeh.Samples.Menu.Unity
{
    public class CacheLifetimeManagerFacts
    {
        [Fact]
        public void SutIsLifetimeManager()
        {
            // Fixture setup
            var dummyLease = new Mock<ILease>().Object;
            // Exercise system
            var sut = new CacheLifetimeManager(dummyLease);
            // Verify outcome
            Assert.IsAssignableFrom<LifetimeManager>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullLeaseThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new CacheLifetimeManager(null));
            // Teardown
        }

        [Fact]
        public void LeaseIsCorrect()
        {
            // Fixture setup
            var expectedLease = new Mock<ILease>().Object;
            var sut = new CacheLifetimeManager(expectedLease);
            // Exercise system
            ILease result = sut.Lease;
            // Verify outcome
            Assert.Equal(expectedLease, result);
            // Teardown
        }

        [Fact]
        public void GetValueReturnsNullWhenFirstUsed()
        {
            // Fixture setup
            var dummyLease = new Mock<ILease>().Object;
            var sut = new CacheLifetimeManager(dummyLease);
            // Exercise system
            var result = sut.GetValue();
            // Verify outcome
            Assert.Null(result);
            // Teardown
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void GetValueReturnsCorrectResultWhenHoldingValue(bool isExpired)
        {
            // Fixture setup
            var value = new object();
            var expectedResult = isExpired ? null : value;

            var leaseStub = new Mock<ILease>();
            leaseStub.SetupGet(l => l.IsExpired).Returns(isExpired);
            var sut = new CacheLifetimeManager(leaseStub.Object);
            // Exercise system
            sut.SetValue(value);
            var result = sut.GetValue();
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Fact]
        public void SetValueRenewsLease()
        {
            // Fixture setup
            var leaseMock = new Mock<ILease>();
            var sut = new CacheLifetimeManager(leaseMock.Object);
            // Exercise system
            var dummyValue = new object();
            sut.SetValue(dummyValue);
            // Verify outcome
            leaseMock.Verify(l => l.Renew());
            // Teardown
        }

        [Fact]
        public void RemoveValueRemovesValueWhenLeaseIsExpired()
        {
            // Fixture setup
            var leaseStub = new Mock<ILease>();
            leaseStub.SetupGet(l => l.IsExpired).Returns(true);

            var sut = new CacheLifetimeManager(leaseStub.Object);
            sut.SetValue(new object());
            // Exercise system
            sut.RemoveValue();
            // Verify outcome
            var result = sut.GetValue();
            Assert.Null(result);
            // Teardown
        }

        [Fact]
        public void RemoveValueDoesNotRemoveValueWhenLeaseIsNotExpired()
        {
            // Fixture setup
            var leaseStub = new Mock<ILease>();
            leaseStub.SetupGet(l => l.IsExpired).Returns(false);

            var expected = new object();

            var sut = new CacheLifetimeManager(leaseStub.Object);
            sut.SetValue(expected);
            // Exercise system
            sut.RemoveValue();
            // Verify outcome
            var result = sut.GetValue();
            Assert.Equal(expected, result);
            // Teardown
        }

        [Fact]
        public void RemoveValueDisposesValue()
        {
            // Fixture setup
            var leaseStub = new Mock<ILease>();
            leaseStub.SetupGet(l => l.IsExpired).Returns(true);

            var sut = new CacheLifetimeManager(leaseStub.Object);

            var valueMock = new Mock<IDisposable>();
            sut.SetValue(valueMock.Object);
            // Exercise system
            sut.RemoveValue();
            // Verify outcome
            valueMock.Verify(d => d.Dispose());
            // Teardown
        }

        [Fact]
        public void SutIsDisposable()
        {
            // Fixture setup
            var dummyLease = new Mock<ILease>().Object;
            // Exercise system
            var sut = new CacheLifetimeManager(dummyLease);
            // Verify outcome
            Assert.IsAssignableFrom<IDisposable>(sut);
            // Teardown
        }

        [Fact]
        public void DisposeRemovesValue()
        {
            // Fixture setup
            var dummyLease = new Mock<ILease>().Object;
            var sut = new CacheLifetimeManager(dummyLease);
            sut.SetValue(new object());
            // Exercise system
            sut.Dispose();
            // Verify outcome
            var result = sut.GetValue();
            Assert.Null(result);
            // Teardown
        }

        [Fact]
        public void DisposeDisposesValue()
        {
            // Fixture setup
            var dummyLease = new Mock<ILease>().Object;
            var sut = new CacheLifetimeManager(dummyLease);

            var valueMock = new Mock<IDisposable>();
            sut.SetValue(valueMock.Object);
            // Exercise system
            sut.Dispose();
            // Verify outcome
            valueMock.Verify(d => d.Dispose());
            // Teardown
        }

        [Fact]
        public void CloneReturnsNewInstance()
        {
            // Fixture setup
            var dummyLease = new Mock<ILease>().Object;
            var sut = new CacheLifetimeManager(dummyLease);
            // Exercise system
            CacheLifetimeManager result = sut.Clone();
            // Verify outcome
            Assert.NotSame(sut, result);
            // Teardown
        }
    }
}
