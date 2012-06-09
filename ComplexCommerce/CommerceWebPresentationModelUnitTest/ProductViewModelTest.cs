using Ploeh.AutoFixture;
using Ploeh.Samples.Commerce.Domain;
using Ploeh.Samples.Commerce.Web.PresentationModel.Models;
using Xunit;

namespace Ploeh.Samples.Commerce.WebPresentationModelUnitTest
{
    public class ProductViewModelTest
    {
        [Fact]
        public void IdWillMatchConstructorArgument()
        {
            // Fixture setup
            var fixture = new Fixture();
            var product = fixture.CreateAnonymous<Product>();
            var expectedId = product.Id;
            var sut = fixture.Build<ProductViewModel>()
                .FromFactory(() => new ProductViewModel(product))
                .OmitAutoProperties()
                .CreateAnonymous();
            // Exercise system
            int result = sut.Id;
            // Verify outcome
            Assert.Equal<int>(expectedId, result);
            // Teardown
        }

        [Fact]
        public void NameIsWellBehavedWritableProperty()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.CreateAnonymous<ProductViewModel>();
            string expectedName = fixture.CreateAnonymous("Name");
            // Exercise system
            sut.Name = expectedName;
            string result = sut.Name;
            // Verify outcome
            Assert.Equal<string>(expectedName, result);
            // Teardown
        }

        [Fact]
        public void UnitPriceIsWellBehavedWritableProperty()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.CreateAnonymous<ProductViewModel>();
            Money expectedPrice = fixture.CreateAnonymous<Money>();
            // Exercise system
            sut.UnitPrice = expectedPrice;
            Money result = sut.UnitPrice;
            // Verify outcome
            Assert.Equal<Money>(expectedPrice, result);
            // Teardown
        }

        [Fact]
        public void SummaryTextWillBeCorrect()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.CreateAnonymous<ProductViewModel>();
            string expectedSummaryText = sut.Name + " (" + sut.UnitPrice.ToString() + ")";
            // Exercise system
            string result = sut.SummaryText;
            // Verify outcome
            Assert.Equal<string>(expectedSummaryText, result);
            // Teardown
        }

        [Fact]
        public void NameWillMatchConstructorArgument()
        {
            // Fixture setup
            var fixture = new Fixture();
            var product = fixture.CreateAnonymous<Product>();
            var sut = fixture.Build<ProductViewModel>()
                .FromFactory(() => new ProductViewModel(product))
                .OmitAutoProperties()
                .CreateAnonymous();

            var expectedName = product.Name;
            // Exercise system
            var result = sut.Name;
            // Verify outcome
            Assert.Equal<string>(expectedName, result);
            // Teardown
        }

        [Fact]
        public void UnitPriceWillMatchConstructorArgument()
        {
            // Fixture setup
            var fixture = new Fixture();
            var product = fixture.CreateAnonymous<Product>();
            var sut = fixture.Build<ProductViewModel>()
                .FromFactory(() => new ProductViewModel(product))
                .OmitAutoProperties()
                .CreateAnonymous();

            var expectedUnitPrice = product.UnitPrice;
            // Exercise system
            var result = sut.UnitPrice;
            // Verify outcome
            Assert.Equal<Money>(expectedUnitPrice, result);
            // Teardown
        }
    }
}
