using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.Threading;

namespace Ploeh.Samples.CacheModel.UnitTest
{
    public class SlidingLeaseFacts
    {
        [Fact]
        public void SutIsLease()
        {
            // Fixture setup
            var dummyTimeout = TimeSpan.FromMinutes(2);
            // Exercise system
            var sut = new SlidingLease(dummyTimeout);
            // Verify outcome
            Assert.IsAssignableFrom<ILease>(sut);
            // Teardown
        }

        [Fact]
        public void TimeoutIsCorrect()
        {
            // Fixture setup
            var expectedTimeout = TimeSpan.FromMinutes(1);
            var sut = new SlidingLease(expectedTimeout);
            // Exercise system
            TimeSpan result = sut.Timeout;
            // Verify outcome
            Assert.Equal(expectedTimeout, result);
            // Teardown
        }

        [Fact]
        public void IsExpiredIsCorrectWhenTimoutIsNotExceeded()
        {
            // Fixture setup
            var sut = new SlidingLease(TimeSpan.FromDays(1));
            // Exercise system
            var result = sut.IsExpired;
            // Verify outcome
            Assert.False(result);
            // Teardown
        }

        [Fact]
        public void IsExpiredIsCorrectWhenTimeoutIsExceeded()
        {
            // Fixture setup
            var sut = new SlidingLease(TimeSpan.FromTicks(100));
            Thread.Sleep(TimeSpan.FromMilliseconds(1));
            // Exercise system
            var result = sut.IsExpired;
            // Verify outcome
            Assert.True(result);
            // Teardown
        }

        [Fact]
        public void IsExpiredIsCorrectAfterRenewal()
        {
            // Fixture setup
            var sut = new SlidingLease(TimeSpan.FromMilliseconds(10));
            Thread.Sleep(TimeSpan.FromMilliseconds(20));
            // Exercise system
            sut.Renew();
            // Verify outcome
            Assert.False(sut.IsExpired);
            // Teardown
        }
    }
}
