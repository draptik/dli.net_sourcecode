using Ploeh.Samples.Commerce.Domain;

namespace Ploeh.Samples.Commerce.Web.PresentationModel.Models
{
    public class ProductViewModel
    {
        private readonly int id;

        public ProductViewModel(Product product)
        {
            this.id = product.Id;
            this.Name = product.Name;
            this.UnitPrice = product.UnitPrice;
        }

        public int Id
        {
            get { return this.id; }
        }

        public string Name { get; set; }

        public string SummaryText
        {
            get
            {
                return string.Format("{0} ({1:C})",
                    this.Name, this.UnitPrice); 
            }
        }

        public Money UnitPrice { get; set; }
    }
}
