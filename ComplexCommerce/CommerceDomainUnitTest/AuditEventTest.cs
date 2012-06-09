using System;
using System.Security.Principal;
using System.Threading;
using Ploeh.AutoFixture;
using Ploeh.Samples.Commerce.Domain;
using Xunit;
using Xunit.Extensions;
using Ploeh.AutoFixture.Xunit;

namespace Ploeh.Samples.Commerce.DomainUnitTest
{
    public class AuditEventTest : IDisposable
    {
        private IPrincipal originalPrincipal;

        public AuditEventTest()
        {
            this.originalPrincipal = Thread.CurrentPrincipal;
        }

        [Fact]
        public void TimestampIsCorrect()
        {
            // Fixture setup
            var fixture = new Fixture();
            var before = DateTimeOffset.Now;
            var sut = fixture.CreateAnonymous<AuditEvent>();
            // Exercise system
            DateTimeOffset result = sut.Timestamp;
            // Verify outcome
            var after = DateTimeOffset.Now;
            Assert.True(before <= result && result <= after);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void NameIsCorrect([Frozen]string expectedName, AuditEvent sut)
        {
            // Fixture setup
            // Exercise system
            string result = sut.Name;
            // Verify outcome
            Assert.Equal(expectedName, result);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void DataIsCorrect([Frozen]object expectedData, AuditEvent sut)
        {
            // Fixture setup
            // Exercise system
            var result = sut.Data;
            // Verify outcome
            Assert.Equal(expectedData, result);
            // Teardown
        }

        [Fact]
        public void IdentityIsCorrect()
        {
            // Fixture setup
            var fixture = new Fixture();
            var expectedIdentity = fixture.CreateAnonymous<GenericIdentity>();
            Thread.CurrentPrincipal = new GenericPrincipal(expectedIdentity, null);

            var sut = fixture.CreateAnonymous<AuditEvent>();
            // Exercise system
            IIdentity result = sut.Identity;
            // Verify outcome
            Assert.Equal(expectedIdentity, result);
            // Teardown
        }

        #region IDisposable Members

        public void Dispose()
        {
            Thread.CurrentPrincipal = this.originalPrincipal;
        }

        #endregion
    }
}
