using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StructureMap.Configuration.DSL;
using StructureMap;
using Ploeh.Samples.Commerce.Domain;
using Ploeh.Samples.Commerce.Data.Sql;
using System.Configuration;
using Ploeh.Samples.Commerce.Web.PresentationModel.Controllers;

namespace Ploeh.Samples.Commerce.Web.StructureMap
{
    /* This should really be a Registry, but because the code in question appears in the book where
     * it does, it's less confusing if the code sample acts on a 'container' variable than on
     * 'this', which would have been the case if this had been a Registry. */
    public static class CommerceCodeAsConfiguration
    {
        public static void Configure(IContainer container)
        {
            container.Configure(c =>
            {
                c.For<IBasketService>().Use<BasketService>();
                c.For<BasketDiscountPolicy>()
                    .Use<RepositoryBasketDiscountPolicy>();

                string connectionString =
                    ConfigurationManager.ConnectionStrings
                    ["CommerceObjectContext"].ConnectionString;
                c.For<BasketRepository>().Use<SqlBasketRepository>()
                    .Ctor<string>().Is(connectionString);
                c.For<DiscountRepository>().Use<SqlDiscountRepository>()
                    .Ctor<string>().Is(connectionString);
                c.For<ProductRepository>().Use<SqlProductRepository>()
                    .Ctor<string>().Is(connectionString);
                c.For<CurrencyProvider>().Use<SqlCurrencyProvider>()
                    .Ctor<string>().Is(connectionString);
            });

            container.Configure(c =>
            {
                c.For<IFormsAuthentication>().Use(new FormsAuthenticationService());
                c.For<IMembershipService>().Use(new AccountMembershipService());
            });
        }
    }
}