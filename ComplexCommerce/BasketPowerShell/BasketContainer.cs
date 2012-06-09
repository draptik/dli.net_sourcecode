using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.Commerce.BasketPowerShellModel;
using Ploeh.Samples.Commerce.Domain;
using Ploeh.Samples.Commerce.Data.Sql;

namespace Ploeh.Samples.Commerce.BasketPowerShell
{
    internal static class BasketContainer
    {
        private const string connectionString = "metadata=res://*/CommerceModel.csdl|res://*/CommerceModel.ssdl|res://*/CommerceModel.msl;provider=System.Data.SqlClient;provider connection string=\"Data Source=.;Initial Catalog=ComplexCommerce;Integrated Security=True;MultipleActiveResultSets=True\"";

        internal static BasketManager ResolveManager()
        {
            BasketRepository basketRepository = 
                new SqlBasketRepository(
                    BasketContainer.connectionString);
            DiscountRepository discountRepository =
                new SqlDiscountRepository(
                    BasketContainer.connectionString);

            BasketDiscountPolicy discountPolicy = 
                new RepositoryBasketDiscountPolicy(
                    discountRepository);

            IBasketService basketService = 
                new BasketService(basketRepository,
                    discountPolicy);

            return new BasketManager(basketService);
        }
    }
}
