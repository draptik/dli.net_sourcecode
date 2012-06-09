using System;
using Ploeh.AutoFixture;
using Ploeh.Samples.Commerce.Domain;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.Samples.Commerce.DomainUnitTest
{
    public class DefaultTimeProviderTest
    {
        [Theory, AutoMoqData]
        public void SutIsTimeProvider(DefaultTimeProvider sut)
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.IsAssignableFrom<TimeProvider>(sut);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void UtcNowReturnsCurrentTime(DefaultTimeProvider sut)
        {
            // Fixture setup
            var before = DateTime.UtcNow;
            // Exercise system
            DateTime result = sut.UtcNow;
            // Verify outcome
            var after = DateTime.UtcNow;
            Assert.True(before <= result && result <= after, "UtcNow should reflect current time");
            // Teardown
        }
    }
}
