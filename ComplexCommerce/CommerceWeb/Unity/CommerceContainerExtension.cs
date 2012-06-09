using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.Unity;
using System.Configuration;
using Ploeh.Samples.Commerce.Domain;
using Ploeh.Samples.Commerce.Data.Sql;
using Ploeh.Samples.Commerce.Web.PresentationModel.Controllers;

namespace Ploeh.Samples.Commerce.Web.Unity
{
    public class CommerceContainerExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            string connectionString =
                ConfigurationManager.ConnectionStrings
                ["CommerceObjectContext"].ConnectionString;
            var sqlCtorParam = new InjectionConstructor(connectionString);

            this.Container.RegisterType<ProductRepository, SqlProductRepository>(new PerResolveLifetimeManager(), sqlCtorParam);
            this.Container.RegisterType<BasketRepository, SqlBasketRepository>(new PerResolveLifetimeManager(), sqlCtorParam);
            this.Container.RegisterType<DiscountRepository, SqlDiscountRepository>(new PerResolveLifetimeManager(), sqlCtorParam);

            this.Container.RegisterType<BasketDiscountPolicy, RepositoryBasketDiscountPolicy>(new PerResolveLifetimeManager());

            this.Container.RegisterType<IBasketService, BasketService>(new PerResolveLifetimeManager());

            this.Container.RegisterType<CurrencyProvider, SqlCurrencyProvider>(new PerResolveLifetimeManager(), sqlCtorParam);

            this.Container.RegisterType<AccountController>(new InjectionConstructor());
        }
    }
}
