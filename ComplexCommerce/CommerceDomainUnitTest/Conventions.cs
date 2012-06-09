using System;
using System.Linq;
using Ploeh.Samples.Commerce.Domain;
using Xunit;

namespace Ploeh.Samples.Commerce.DomainUnitTest
{
    public class Conventions
    {
        [Fact]
        public void SutShouldNotReferenceSqlDataAccess()
        {
            // Fixture setup
            Type sutRepresentative = typeof(Basket);
            var unwanted = "Ploeh.Samples.Commerce.Data.Sql";
            // Exercise system
            var references =
                sutRepresentative.Assembly
                .GetReferencedAssemblies();
            // Verify outcome
            Assert.False(references.Any(a => a.Name == unwanted),
                string.Format(
                "{0} should not be referenced by SUT", unwanted));
            // Teardown
        }
    }
}
