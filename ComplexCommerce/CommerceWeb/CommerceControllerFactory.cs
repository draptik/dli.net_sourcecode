using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Ploeh.Samples.Commerce.Domain;
using Ploeh.Samples.Commerce.Web.PresentationModel.Controllers;
using System.Configuration;
using Ploeh.Samples.Commerce.Data.Sql;

namespace Ploeh.Samples.Commerce.Web
{
    public class CommerceControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(
            RequestContext requestContext, Type controllerType)
        {
            string connectionString =
                ConfigurationManager.ConnectionStrings
                ["CommerceObjectContext"].ConnectionString;

            var productRepository =
                new SqlProductRepository(connectionString);
            var basketRepository =
                new SqlBasketRepository(connectionString);
            var discountRepository =
                new SqlDiscountRepository(connectionString);

            var discountPolicy =
                new RepositoryBasketDiscountPolicy(
                    discountRepository);

            var basketService =
                new BasketService(basketRepository,
                    discountPolicy);

            var currencyProvider = new CachingCurrencyProvider(
                new SqlCurrencyProvider(connectionString),
                TimeSpan.FromHours(1));

            if (controllerType == typeof(BasketController))
            {
                return new BasketController(
                    basketService, currencyProvider);
            }
            if (controllerType == typeof(HomeController))
            {
                return new HomeController(
                    productRepository, currencyProvider);
            }

            return base.GetControllerInstance(
                requestContext, controllerType);
        }
    }
}
