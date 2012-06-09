using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.ProductManagement.PresentationLogic.Wpf
{
    public interface IProductManagementAgent
    {
        void DeleteProduct(int productId);

        void InsertProduct(ProductEditorViewModel product);

        IEnumerable<ProductViewModel> SelectAllProducts();

        void UpdateProduct(ProductEditorViewModel product);
    }
}
