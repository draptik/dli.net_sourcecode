using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.Samples.ProductManagement.PresentationLogic.Wpf;
using Xunit;

namespace Ploeh.Samples.ProductManagement.WcfAgent.UnitTest
{
    public class CircuitBreakerProductManagementAgentTest
    {
        [Fact]
        public void SutIsProductManagementAgent()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            // Exercise system
            var sut = fixture.CreateAnonymous<CircuitBreakerProductManagementAgent>();
            // Verify outcome
            Assert.IsAssignableFrom<IProductManagementAgent>(sut);
            // Teardown
        }

        [Fact]
        public void DeleteProductWillDeleteProductFromAgent()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            var id = fixture.CreateAnonymous<int>();
            var agentMock = fixture.Freeze<Mock<IProductManagementAgent>>();
            var sut = fixture.CreateAnonymous<CircuitBreakerProductManagementAgent>();
            // Exercise system
            sut.DeleteProduct(id);
            // Verify outcome
            agentMock.Verify(a => a.DeleteProduct(id));
            // Teardown
        }

        [Fact]
        public void DeleteProductWillOrchestrateBreakerCorrectly()
        {
            // Fixture setup
            var fixture = new WcfFixture();

            var spy = new List<int>();
            var breakerStub = fixture.Freeze<Mock<ICircuitBreaker>>();
            breakerStub.Setup(cb => cb.Guard()).Callback(() => spy.Add(1));
            fixture.Freeze<Mock<IProductManagementAgent>>().Setup(a => a.DeleteProduct(It.IsAny<int>())).Callback(() => spy.Add(2));
            breakerStub.Setup(b => b.Succeed()).Callback(() => spy.Add(3));

            var sut = fixture.CreateAnonymous<CircuitBreakerProductManagementAgent>();
            // Exercise system
            fixture.Do((int id) => sut.DeleteProduct(id));
            // Verify outcome
            Assert.True(Enumerable.Range(1, 3).SequenceEqual(spy));
            // Teardown
        }

        [Fact]
        public void DeleteProductWillTripBreakerWhenAgentThrows()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            var e = fixture.CreateAnonymous<TimeoutException>();

            var breakerMock = fixture.Freeze<Mock<ICircuitBreaker>>();

            fixture.Freeze<Mock<IProductManagementAgent>>().Setup(a => a.DeleteProduct(It.IsAny<int>())).Throws(e);

            var sut = fixture.CreateAnonymous<CircuitBreakerProductManagementAgent>();
            // Exercise system
            try { fixture.Do((int id) => sut.DeleteProduct(id)); }
            // Verify outcome
            catch (TimeoutException) { }
            finally { breakerMock.Verify(b => b.Trip(e)); }
            // Teardown
        }

        [Fact]
        public void DeleteProductWillRethrow()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            var expectedException = fixture.CreateAnonymous<InvalidOperationException>();
            fixture.Freeze<Mock<IProductManagementAgent>>().Setup(a => a.DeleteProduct(It.IsAny<int>())).Throws(expectedException);

            var verified = false;
            var sut = fixture.CreateAnonymous<CircuitBreakerProductManagementAgent>();
            // Exercise system
            try { fixture.Do((int id) => sut.DeleteProduct(id)); }
            // Verify outcome
            catch (InvalidOperationException e) { verified = e == expectedException; }
            finally { Assert.True(verified); }
            // Teardown
        }

        [Fact]
        public void InsertProductWillInsertProductInAgent()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            var product = fixture.CreateAnonymous<ProductEditorViewModel>();
            var agentMock = fixture.Freeze<Mock<IProductManagementAgent>>();
            var sut = fixture.CreateAnonymous<CircuitBreakerProductManagementAgent>();
            // Exercise system
            sut.InsertProduct(product);
            // Verify outcome
            agentMock.Verify(a => a.InsertProduct(product));
            // Teardown
        }

        [Fact]
        public void InsertProductWillOrchestrateBreakerCorrectly()
        {
            // Fixture setup
            var fixture = new WcfFixture();

            var spy = new List<int>();
            var breakerStub = fixture.Freeze<Mock<ICircuitBreaker>>();
            breakerStub.Setup(cb => cb.Guard()).Callback(() => spy.Add(1));
            fixture.Freeze<Mock<IProductManagementAgent>>().Setup(a => a.InsertProduct(It.IsAny<ProductEditorViewModel>())).Callback(() => spy.Add(2));
            breakerStub.Setup(b => b.Succeed()).Callback(() => spy.Add(3));

            var sut = fixture.CreateAnonymous<CircuitBreakerProductManagementAgent>();
            // Exercise system
            fixture.Do((ProductEditorViewModel p) => sut.InsertProduct(p));
            // Verify outcome
            Assert.True(Enumerable.Range(1, 3).SequenceEqual(spy));
            // Teardown
        }

        [Fact]
        public void InsertProductWillTripBreakerWhenAgentThrows()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            var e = fixture.CreateAnonymous<TimeoutException>();

            var breakerMock = fixture.Freeze<Mock<ICircuitBreaker>>();

            fixture.Freeze<Mock<IProductManagementAgent>>().Setup(a => a.InsertProduct(It.IsAny<ProductEditorViewModel>())).Throws(e);

            var sut = fixture.CreateAnonymous<CircuitBreakerProductManagementAgent>();
            // Exercise system
            try { fixture.Do((ProductEditorViewModel p) => sut.InsertProduct(p)); }
            // Verify outcome
            catch (TimeoutException) { }
            finally { breakerMock.Verify(b => b.Trip(e)); }
            // Teardown
        }

        [Fact]
        public void InsertProductWillRethrow()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            var expectedException = fixture.CreateAnonymous<InvalidOperationException>();
            fixture.Freeze<Mock<IProductManagementAgent>>().Setup(a => a.InsertProduct(It.IsAny<ProductEditorViewModel>())).Throws(expectedException);

            var verified = false;
            var sut = fixture.CreateAnonymous<CircuitBreakerProductManagementAgent>();
            // Exercise system
            try { fixture.Do((ProductEditorViewModel p) => sut.InsertProduct(p)); }
            // Verify outcome
            catch (InvalidOperationException e) { verified = e == expectedException; }
            finally { Assert.True(verified); }
            // Teardown
        }

        [Fact]
        public void SelectAllProductsWillReturnCorrectResult()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            var expectedProducts = fixture.CreateMany<ProductViewModel>().ToList();
            fixture.Freeze<Mock<IProductManagementAgent>>().Setup(a => a.SelectAllProducts()).Returns(expectedProducts);
            var sut = fixture.CreateAnonymous<CircuitBreakerProductManagementAgent>();
            // Exercise system
            var result = sut.SelectAllProducts();
            // Verify outcome
            Assert.Equal(expectedProducts, result);
            // Teardown
        }

        [Fact]
        public void SelectAllProductsWillOrchestrateBreakerCorrectly()
        {
            // Fixture setup
            var fixture = new WcfFixture();

            var spy = new List<int>();
            var breakerStub = fixture.Freeze<Mock<ICircuitBreaker>>();
            breakerStub.Setup(cb => cb.Guard()).Callback(() => spy.Add(1));
            fixture.Freeze<Mock<IProductManagementAgent>>().Setup(a => a.SelectAllProducts()).Callback(() => spy.Add(2)).Returns(fixture.CreateMany<ProductViewModel>());
            breakerStub.Setup(b => b.Succeed()).Callback(() => spy.Add(3));

            var sut = fixture.CreateAnonymous<CircuitBreakerProductManagementAgent>();
            // Exercise system
            sut.SelectAllProducts().ToList();
            // Verify outcome
            Assert.True(Enumerable.Range(1, 3).SequenceEqual(spy));
            // Teardown
        }

        [Fact]
        public void SelectAllProductsWillTripBreakerWhenAgentThrows()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            var e = fixture.CreateAnonymous<TimeoutException>();

            var breakerMock = fixture.Freeze<Mock<ICircuitBreaker>>();

            fixture.Freeze<Mock<IProductManagementAgent>>().Setup(a => a.SelectAllProducts()).Throws(e);

            var sut = fixture.CreateAnonymous<CircuitBreakerProductManagementAgent>();
            // Exercise system
            try { sut.SelectAllProducts().ToList(); }
            // Verify outcome
            catch (TimeoutException) { }
            finally { breakerMock.Verify(b => b.Trip(e)); }
            // Teardown
        }

        [Fact]
        public void SelectAllProductsWillRethrow()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            var expectedException = fixture.CreateAnonymous<InvalidOperationException>();
            fixture.Freeze<Mock<IProductManagementAgent>>().Setup(a => a.SelectAllProducts()).Throws(expectedException);

            var verified = false;
            var sut = fixture.CreateAnonymous<CircuitBreakerProductManagementAgent>();
            // Exercise system
            try { sut.SelectAllProducts().ToList(); }
            // Verify outcome
            catch (InvalidOperationException e) { verified = e == expectedException; }
            finally { Assert.True(verified); }
            // Teardown
        }

        [Fact]
        public void UpdateProductWillInsertProductInAgent()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            var product = fixture.CreateAnonymous<ProductEditorViewModel>();
            var agentMock = fixture.Freeze<Mock<IProductManagementAgent>>();
            var sut = fixture.CreateAnonymous<CircuitBreakerProductManagementAgent>();
            // Exercise system
            sut.UpdateProduct(product);
            // Verify outcome
            agentMock.Verify(a => a.UpdateProduct(product));
            // Teardown
        }

        [Fact]
        public void UpdateProductWillOrchestrateBreakerCorrectly()
        {
            // Fixture setup
            var fixture = new WcfFixture();

            var spy = new List<int>();
            var breakerStub = fixture.Freeze<Mock<ICircuitBreaker>>();
            breakerStub.Setup(cb => cb.Guard()).Callback(() => spy.Add(1));
            fixture.Freeze<Mock<IProductManagementAgent>>().Setup(a => a.UpdateProduct(It.IsAny<ProductEditorViewModel>())).Callback(() => spy.Add(2));
            breakerStub.Setup(b => b.Succeed()).Callback(() => spy.Add(3));

            var sut = fixture.CreateAnonymous<CircuitBreakerProductManagementAgent>();
            // Exercise system
            fixture.Do((ProductEditorViewModel p) => sut.UpdateProduct(p));
            // Verify outcome
            Assert.True(Enumerable.Range(1, 3).SequenceEqual(spy));
            // Teardown
        }

        [Fact]
        public void UpdateProductWillTripBreakerWhenAgentThrows()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            var e = fixture.CreateAnonymous<TimeoutException>();

            var breakerMock = fixture.Freeze<Mock<ICircuitBreaker>>();

            fixture.Freeze<Mock<IProductManagementAgent>>().Setup(a => a.UpdateProduct(It.IsAny<ProductEditorViewModel>())).Throws(e);

            var sut = fixture.CreateAnonymous<CircuitBreakerProductManagementAgent>();
            // Exercise system
            try { fixture.Do((ProductEditorViewModel p) => sut.UpdateProduct(p)); }
            // Verify outcome
            catch (TimeoutException) { }
            finally { breakerMock.Verify(b => b.Trip(e)); }
            // Teardown
        }

        [Fact]
        public void UpdateProductWillRethrow()
        {
            // Fixture setup
            var fixture = new WcfFixture();
            var expectedException = fixture.CreateAnonymous<InvalidOperationException>();
            fixture.Freeze<Mock<IProductManagementAgent>>().Setup(a => a.UpdateProduct(It.IsAny<ProductEditorViewModel>())).Throws(expectedException);

            var verified = false;
            var sut = fixture.CreateAnonymous<CircuitBreakerProductManagementAgent>();
            // Exercise system
            try { fixture.Do((ProductEditorViewModel p) => sut.UpdateProduct(p)); }
            // Verify outcome
            catch (InvalidOperationException e) { verified = e == expectedException; }
            finally { Assert.True(verified); }
            // Teardown
        }
    }
}
