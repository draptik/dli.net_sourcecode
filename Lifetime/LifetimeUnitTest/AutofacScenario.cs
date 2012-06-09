using System.Configuration;
using Autofac;
using Ploeh.AutoFixture;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.Samples.Dependency.Lifetime.UnitTest
{
    public class AutofacScenario
    {
        [Fact]
        public void ConfigureContractMapper()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ContractMapper>().As<IContractMapper>();
            var container = builder.Build();
            var mapper = container.Resolve<IContractMapper>();
            Assert.IsType<ContractMapper>(mapper);
        }

        [Theory, AutoMoqData]
        public void ConfigureSqlProductRepositoryWithConnectionString(string connectionString)
        {
            var builder = new ContainerBuilder();
            builder.Register((c, p) => new SqlProductRepository(connectionString)).As<ProductRepository>();
            var container = builder.Build();

            var repository = container.Resolve<ProductRepository>();

            Assert.IsType<SqlProductRepository>(repository);
        }

        [Fact]
        public void ConnectionStringIsNotNull()
        {
            // Fixture setup
            // Exercise system
            var result = ConfigurationManager.ConnectionStrings["CommerceObjectContext"].ConnectionString;
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void ConfigureWithAllPerDependencyInstanceScope()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ContractMapper>()
                .As<IContractMapper>();
            builder.Register((c, p) => 
                new SqlProductRepository(
                    ConfigurationManager
                    .ConnectionStrings["CommerceObjectContext"]
                    .ConnectionString))
                .As<ProductRepository>();
            builder.RegisterType<ProductManagementService>()
                .As<IProductManagementService>();
            var container = builder.Build();

            var service = container.Resolve<IProductManagementService>();

            var service2 = container.Resolve<IProductManagementService>();

            Assert.NotEqual(((ProductManagementService)service).ContractMapper, ((ProductManagementService)service2).ContractMapper);
            Assert.NotEqual(((ProductManagementService)service).Repository, ((ProductManagementService)service2).Repository);
        }

        [Fact]
        public void ExplicitlyConfigureWithAllPerDependencyInstanceScope()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ContractMapper>()
                .As<IContractMapper>()
                .InstancePerDependency();
            builder.Register((c, p) => new SqlProductRepository(ConfigurationManager.ConnectionStrings["CommerceObjectContext"].ConnectionString)).As<ProductRepository>().InstancePerDependency();
            builder.RegisterType<ProductManagementService>().As<IProductManagementService>().InstancePerDependency(); ;
            var container = builder.Build();

            var service = container.Resolve<IProductManagementService>();

            var service2 = container.Resolve<IProductManagementService>();

            Assert.NotEqual(((ProductManagementService)service).ContractMapper, ((ProductManagementService)service2).ContractMapper);
            Assert.NotEqual(((ProductManagementService)service).Repository, ((ProductManagementService)service2).Repository);
        }

        [Theory, AutoMoqData]
        public void ConfigureWithProperSingleInstances(string connectionString)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ContractMapper>()
                .As<IContractMapper>()
                .SingleInstance();
            builder.Register((c, p) => 
                new SqlProductRepository(connectionString))
                .As<ProductRepository>();
            builder.RegisterType<ProductManagementService>()
                .As<IProductManagementService>();
            var container = builder.Build();

            var service = container.Resolve<IProductManagementService>();

            var service2 = container.Resolve<IProductManagementService>();

            Assert.Equal(((ProductManagementService)service).ContractMapper, ((ProductManagementService)service2).ContractMapper);
            Assert.NotEqual(((ProductManagementService)service).Repository, ((ProductManagementService)service2).Repository);
        }
    }
}
