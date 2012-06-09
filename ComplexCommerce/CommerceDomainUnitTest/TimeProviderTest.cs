using System;
using Moq;
using Ploeh.Samples.Commerce.Domain;
using Xunit;

namespace Ploeh.Samples.Commerce.DomainUnitTest
{
    public class TimeProviderTest : IDisposable
    {
        [Fact]
        public void DefaultTimeProviderIsCorrect()
        {
            // Fixture setup
            // Exercise system
            TimeProvider result = TimeProvider.Current;
            // Verify outcome
            Assert.IsAssignableFrom<DefaultTimeProvider>(result);
            // Teardown (implicit)
        }

        [Fact]
        public void CurrentIsWellBehavedWritableProperty()
        {
            // Fixture setup
            TimeProvider expectedTimeProvider = new Mock<TimeProvider>().Object;
            // Exercise system
            TimeProvider.Current = expectedTimeProvider;
            TimeProvider result = TimeProvider.Current;
            // Verify outcome
            Assert.Equal<TimeProvider>(expectedTimeProvider, result);
            // Teardown (implicit)
        }

        [Fact]
        public void SettingCurrentToNullWillThrow()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                TimeProvider.Current = null);
            // Teardown
        }

        #region IDisposable Members

        public void Dispose()
        {
            TimeProvider.ResetToDefault();
        }

        #endregion
    }
}
