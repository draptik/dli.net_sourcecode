using System;
using System.Linq;
using System.Reflection;
using Ploeh.Samples.Commerce.Data.Sql;
using Ploeh.Samples.Commerce.Domain;
using Ploeh.Samples.Commerce.Web.PresentationModel.Controllers;
using Xunit;

namespace Ploeh.SamplesCommerce.IntegrationTest
{
    public class ArchitectureConstraints
    {
        [Fact]
        public void PresentationModuleShouldNotReferenceSqlDataAccess()
        {
            // Fixture setup
            Type presentationRepresentative =
                typeof(HomeController);
            Type sqlRepresentative =
                typeof(SqlProductRepository);
            // Exercise system
            var references =
                presentationRepresentative.Assembly
                .GetReferencedAssemblies();
            // Verify outcome
            AssemblyName sqlAssemblyName =
                sqlRepresentative.Assembly.GetName();
            AssemblyName presentationAssemblyName =
                presentationRepresentative.Assembly.GetName();

            Assert.False(references.Any(a =>
                AssemblyName.ReferenceMatchesDefinition(
                    sqlAssemblyName, a)),
                string.Format(
                    "{0} should not be referenced by {1}",
                    sqlAssemblyName, 
                    presentationAssemblyName));
            // Teardown
        }

        [Fact]
        public void DomainModuleShouldNotReferenceSqlDataAccess()
        {
            // Fixture setup
            Type domainRepresentative = typeof(Basket);
            Type sqlRepresentative =
                typeof(SqlProductRepository);
            // Exercise system
            var references =
                domainRepresentative.Assembly
                .GetReferencedAssemblies();
            // Verify outcome
            AssemblyName sqlAssemblyName =
                sqlRepresentative.Assembly.GetName();
            AssemblyName presentationAssemblyName =
                domainRepresentative.Assembly.GetName();

            Assert.False(references.Any(a =>
                AssemblyName.ReferenceMatchesDefinition(
                    sqlAssemblyName, a)),
                string.Format(
                    "{0} should not be referenced by {1}",
                    sqlAssemblyName, presentationAssemblyName));
            // Teardown
        }
    }
}
