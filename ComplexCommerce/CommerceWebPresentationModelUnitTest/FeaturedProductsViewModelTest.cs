using System.Collections.Generic;
using Ploeh.AutoFixture;
using Ploeh.Samples.Commerce.Web.PresentationModel.Models;
using Xunit;

namespace Ploeh.Samples.Commerce.WebPresentationModelUnitTest
{
    public class FeaturedProductsViewModelTest
    {
        [Fact]
        public void ProductsIsNotNull()
        {
            // Fixture setup
            var fixture = new Fixture();
            var sut = fixture.CreateAnonymous<FeaturedProductsViewModel>();
            // Exercise system
            IEnumerable<ProductViewModel> result = sut.Products;
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }
    }
}
