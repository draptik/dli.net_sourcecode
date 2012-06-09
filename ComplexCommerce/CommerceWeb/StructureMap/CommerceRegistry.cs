using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StructureMap.Configuration.DSL;
using Ploeh.Samples.Commerce.Web.PresentationModel.Controllers;
using System.Web.Mvc;
using Ploeh.Samples.Commerce.Domain;
using System.Configuration;
using Ploeh.Samples.Commerce.Data.Sql;

namespace Ploeh.Samples.Commerce.Web.StructureMap
{
    public class CommerceRegistry : Registry
    {
        public CommerceRegistry()
        {
            this.For<IFormsAuthentication>().Use(new FormsAuthenticationService());
            this.For<IMembershipService>().Use(new AccountMembershipService());

            this.Scan(x =>
            {
                x.AssemblyContainingType<HomeController>();
                x.AddAllTypesOf<IController>();
                x.Include(t => typeof(IController).IsAssignableFrom(t));
                x.Convention<HttpContextScopedConvention>();
            });

            this.Scan(x =>
            {
                x.AssemblyContainingType<BasketService>();
                x.RegisterConcreteTypesAgainstTheFirstInterface();
                x.Include(t => t.Name.EndsWith("Service"));
                x.Convention<SingletonConvention>();
            });
            this.Scan(x =>
            {
                x.AssemblyContainingType<BasketDiscountPolicy>();
                x.Include(t => t.Name.EndsWith("Policy"));
                x.Convention<BaseTypeConvention>();
                x.Convention<SingletonConvention>();
            });

            string connectionString =
                ConfigurationManager.ConnectionStrings
                ["CommerceObjectContext"].ConnectionString;
            this.Scan(x =>
            {
                x.AssemblyContainingType<SqlProductRepository>();
                //x.Include(t => t.Name.StartsWith("Sql"));
                //x.Convention<BaseTypeConvention>();
                //x.Convention<HttpContextScopedConvention>();
                x.With(new ConnectionStringConvention(connectionString));
            });
            //this.For<ProductRepository>().HttpContextScoped().Use<SqlProductRepository>().Ctor<string>().Is(connectionString);
            //this.For<CurrencyProvider>().HttpContextScoped().Use<SqlCurrencyProvider>().Ctor<string>().Is(connectionString);
            //this.For<BasketRepository>().HttpContextScoped().Use<SqlBasketRepository>().Ctor<string>().Is(connectionString);
            //this.For<CampaignRepository>().HttpContextScoped().Use<SqlCampaignRepository>().Ctor<string>().Is(connectionString);
            //this.For<DiscountRepository>().HttpContextScoped().Use<SqlDiscountRepository>().Ctor<string>().Is(connectionString);
            //this.For<Currency>().HttpContextScoped().Use<SqlCurrency>().Ctor<string>().Is(connectionString);
        }
    }
}
