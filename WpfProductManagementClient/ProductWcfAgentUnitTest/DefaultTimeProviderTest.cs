using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Ploeh.Samples.ProductManagement.WcfAgent.UnitTest
{
    public class DefaultTimeProviderTest
    {
        [Fact]
        public void InstanceIsNotNull()
        {
            // Fixture setup
            // Exercise system
            DefaultTimeProvider result = DefaultTimeProvider.Instance;
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void SutIsTimeProvider()
        {
            // Fixture setup
            // Exercise system
            var sut = DefaultTimeProvider.Instance;
            // Verify outcome
            Assert.IsAssignableFrom<TimeProvider>(sut);
            // Teardown
        }

        [Fact]
        public void UtcNowReturnsCurrentTime()
        {
            // Fixture setup
            var sut = DefaultTimeProvider.Instance;
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
