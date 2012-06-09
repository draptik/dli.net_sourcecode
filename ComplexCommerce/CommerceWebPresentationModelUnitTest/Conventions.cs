using System;
using System.Linq;
using Ploeh.Samples.Commerce.Web.PresentationModel.Controllers;
using Xunit;

namespace Ploeh.Samples.Commerce.WebPresentationModelUnitTest
{
    public class Conventions
    {
        [Fact]
        public void SutShouldNotReferenceSqlDataAccess()
        {
            // Fixture setup
            Type sutRepresentative = typeof(HomeController);
            var unwanted = "Ploeh.Samples.Commerce.Data.Sql";
            // Exercise system
            var references =
                sutRepresentative.Assembly
                .GetReferencedAssemblies();
            // Verify outcome
            Assert.False(
                references.Any(a => a.Name == unwanted),
                string.Format(
                    "{0} should not be referenced by SUT",
                    unwanted));
            // Teardown
        }
    }
}
