using System.Collections.Generic;
using System.Linq;

namespace Ploeh.Samples.Commerce.Data.Sql
{
    public class SqlDiscountRepository : Domain.DiscountRepository
    {
        private readonly CommerceObjectContext context;

        public SqlDiscountRepository(string connString)
        {
            this.context = new CommerceObjectContext(connString);
        }

        public override IEnumerable<Domain.Product> GetDiscountedProducts()
        {
            return (from p in this.context.Products
                    where p.DiscountedUnitPrice.HasValue
                    select p).AsEnumerable().Select(p => p.ToDiscountedProduct());
        }
    }
}
