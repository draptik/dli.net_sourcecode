using System.Collections.Generic;
using Ploeh.AutoFixture;
using Ploeh.Samples.Dependency.Lifetime;
using Ploeh.Samples.Lifetime.MvcApplication.Models;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.Samples.Lifetime.MvcApplication.UnitTest.Models
{
    public class HomeIndexViewModelFacts
    {
        [Theory, AutoMvcData]
        public void ProductsIsNotNull(HomeIndexViewModel sut)
        {
            // Fixture setup
            // Exercise system
            IList<Product> result = sut.Products;
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Theory, AutoMvcData]
        public void ProductsRetainsItems(Product product, HomeIndexViewModel sut)
        {
            // Fixture setup
            // Exercise system
            sut.Products.Add(product);
            // Verify outcome
            Assert.Contains(product, sut.Products);
            // Teardown
        }
    }
}
