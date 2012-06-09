using System;
using Ploeh.AutoFixture;
using Xunit;

namespace Ploeh.Samples.ProductManagement.WcfAgent.UnitTest
{
    public class CircuitBreakerTest : IDisposable
    {
        [Fact]
        public void SutIsCircuitBreaker()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            // Exercise system
            var sut = fixture.CreateAnonymous<CircuitBreaker>();
            // Verify outcome
            Assert.IsAssignableFrom<ICircuitBreaker>(sut);
            // Teardown
        }

        [Fact]
        public void DefaultStateIsClosed()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            var sut = fixture.CreateAnonymous<CircuitBreaker>();
            // Exercise system
            ICircuitState result = sut.State;
            // Verify outcome
            Assert.IsAssignableFrom<ClosedCircuitState>(result);
            // Teardown
        }

        [Fact]
        public void GuardDoesNotThrowWhenStateIsClosed()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            var sut = fixture.CreateAnonymous<CircuitBreaker>();
            // Exercise system
            sut.Guard();
            // Verify outcome (no exception indicates success);
            // Teardown
        }

        [Fact]
        public void SucceedDoesNotThrowWhenStateIsClosed()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            var sut = fixture.CreateAnonymous<CircuitBreaker>();
            // Exercise system
            sut.Succeed();
            // Verify outcome (no exception indicates success);
            // Teardown
        }

        [Fact]
        public void TripWillTransitionToOpen()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            var sut = fixture.CreateAnonymous<CircuitBreaker>();
            // Exercise system
            fixture.Do((Exception e) => sut.Trip(e));
            // Verify outcome
            Assert.IsAssignableFrom<OpenCircuitState>(sut.State);
            // Teardown
        }

        [Fact]
        public void GuardWillThrowWhenStateIsOpen()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            fixture.Inject<TimeSpan>(1.Minutes());
            var sut = fixture.CreateAnonymous<CircuitBreaker>();
            sut.PutInOpenState();
            // Exercise system and verify outcome
            Assert.Throws<InvalidOperationException>(() =>
                sut.Guard());
            // Teardown
        }

        [Fact]
        public void SucceedDoesNotThrowWhenStateIsOpen()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            var sut = fixture.CreateAnonymous<CircuitBreaker>();
            sut.PutInOpenState();
            // Exercise system
            sut.Succeed();
            // Verify outcome (no exception indicates success);
            // Teardown
        }

        [Fact]
        public void GuardWillTransitionToHalfOpenWhenStateIsOpenAndTimeoutIsReached()
        {
            // Fixture setup
            var fixture = new WcfFixture();

            DateTime.Now.Freeze();

            fixture.Inject(1.Minutes());
            var sut = fixture.CreateAnonymous<CircuitBreaker>();
            sut.PutInOpenState();

            2.Minutes().Pass();
            // Exercise system
            sut.Guard();
            // Verify outcome
            Assert.IsAssignableFrom<HalfOpenCircuitState>(sut.State);
            // Teardown
        }

        [Fact]
        public void TripWillTransitionToOpenWhenStateIsHalfOpen()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            fixture.Inject(1.Minutes());
            var sut = fixture.CreateAnonymous<CircuitBreaker>();
            sut.PutInHalfOpenState();
            // Exercise system
            fixture.Do((Exception e) => sut.Trip(e));
            // Verify outcome
            Assert.IsAssignableFrom<OpenCircuitState>(sut.State);
            // Teardown
        }

        [Fact]
        public void GuardWillNotThrowWhenStateIsHalfOpen()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            fixture.Inject(1.Minutes());
            var sut = fixture.CreateAnonymous<CircuitBreaker>();
            sut.PutInHalfOpenState();
            // Exercise system
            sut.Guard();
            // Verify outcome (no exception indicates success)
            // Teardown
        }

        [Fact]
        public void SucceedWilTransitionToClosedWhenStateIsHalfOpen()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            fixture.Inject(1.Minutes());
            var sut = fixture.CreateAnonymous<CircuitBreaker>();
            sut.PutInHalfOpenState();
            // Exercise system
            sut.Succeed();
            // Verify outcome
            Assert.IsAssignableFrom<ClosedCircuitState>(sut.State);
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
