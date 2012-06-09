using System;

namespace Ploeh.Samples.Dependency.Lifetime
{
    // This class doesn't really do anything meaningful. It just exists to look good on the pages
    // of the book, and to verify certain interactions.
    public class DiscountCampaign
    {
        private readonly DiscountRepository repository;

        public DiscountCampaign(DiscountRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }

            this.repository = repository;
        }

        public DiscountRepository Repository
        {
            get { return this.repository; }
        }

        public virtual void AddProduct(Product product)
        {
            this.repository.Products.Add(product);
        }
    }
}
