using Ploeh.AutoFixture;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.Samples.Dependency.Lifetime.UnitTest
{
    public class ProductTest
    {
        [Theory, AutoMoqData]
        public void NameIsProperWritableProperty(string expectedName, Product sut)
        {
            // Fixture setup
            // Exercise system
            sut.Name = expectedName;
            string result = sut.Name;
            // Verify outcome
            Assert.Equal(expectedName, result);
            // Teardown
        }
    }
}
