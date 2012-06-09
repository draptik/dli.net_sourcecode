using Ploeh.AutoFixture;
using Ploeh.SemanticComparison.Fluent;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.Samples.ProductManagement.PresentationLogic.Wpf.UnitTest
{
    public class ProductViewModelTest
    {
        [Theory, AutoMoqData]
        public void IdIsProperWritableProperty(int expectedId, ProductViewModel sut)
        {
            // Fixture setup
            // Exercise system
            sut.Id = expectedId;
            int result = sut.Id;
            // Verify outcome
            Assert.Equal(expectedId, result);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void NameIsProperWritabeProperty(string expectedName, ProductViewModel sut)
        {
            // Fixture setup
            // Exercise system
            sut.Name = expectedName;
            string result = sut.Name;
            // Verify outcome
            Assert.Equal(expectedName, result);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void PriceIsProperWritableProperty(MoneyViewModel expectedPrice, ProductViewModel sut)
        {
            // Fixture setup
            // Exercise system
            sut.UnitPrice = expectedPrice;
            MoneyViewModel result = sut.UnitPrice;
            // Verify outcome
            Assert.Equal(expectedPrice, result);
            // Teardown
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsSelectedIsProperWritableProperty(bool expectedValue)
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.CreateAnonymous<ProductViewModel>();
            // Exercise system
            sut.IsSelected = expectedValue;
            bool result = sut.IsSelected;
            // Verify outcome
            Assert.Equal(expectedValue, result);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void EditWillReturnCorrectResult(ProductViewModel sut)
        {
            // Fixture setup
            var expectedEditor = sut.AsSource().OfLikeness<ProductEditorViewModel>()
                .With(d => d.Currency).EqualsWhen((s, d) => s.UnitPrice.CurrencyCode == d.Currency)
                .With(d => d.Price).EqualsWhen((s, d) => s.UnitPrice.Amount.ToString("F") == d.Price)
                .Without(d => d.Error)
                .Without(d => d.IsValid)
                .Without(d => d.Title);
            // Exercise system
            ProductEditorViewModel result = sut.Edit();
            // Verify outcome
            Assert.True(expectedEditor.Equals(result));
            // Teardown
        }
    }
}
