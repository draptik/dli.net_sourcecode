using System.Configuration;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Ploeh.Samples.Commerce.Data.Sql;
using Ploeh.Samples.Commerce.Domain;
using Ploeh.Samples.Commerce.Web.PresentationModel.Controllers;

namespace Ploeh.Samples.Commerce.Web.Windsor
{
    public class CommerceWindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container,
            IConfigurationStore store)
        {
            container.Register(AllTypes
                .FromAssemblyContaining<HomeController>()
                .BasedOn<IController>()
                .Configure(r => r.LifeStyle.PerWebRequest));

            container.Register(AllTypes
                .FromAssemblyContaining<BasketService>()
                .Where(t => t.Name.EndsWith("Service"))
                .WithService
                .AllInterfaces());
            container.Register(AllTypes
                .FromAssemblyContaining<BasketDiscountPolicy>()
                .Where(t => t.Name.EndsWith("Policy"))
                .WithService
                .Select((t, b) => new[] { t.BaseType }));

            string connectionString =
                ConfigurationManager.ConnectionStrings
                ["CommerceObjectContext"].ConnectionString;
            container.Register(AllTypes
                .FromAssemblyContaining<SqlProductRepository>()
                .Where(t => t.Name.StartsWith("Sql"))
                .WithService
                .Select((t, b) => new[] { t.BaseType })
                .Configure(r => r.LifeStyle.PerWebRequest
                    .DependsOn((new
                    {
                        connString = connectionString
                    }))));
        }
    }
}