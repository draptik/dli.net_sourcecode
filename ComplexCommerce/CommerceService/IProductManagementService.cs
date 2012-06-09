using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Ploeh.Samples.CommerceService
{
    [ServiceContract(Namespace = "urn:ploeh:productMgtSrvc")]
    public interface IProductManagementService
    {
        [OperationContract]
        void DeleteProduct(int productId);

        [OperationContract]
        void InsertProduct(ProductContract product);

        [OperationContract]
        ProductContract SelectProduct(int productId);

        [OperationContract]
        ProductContract[] SelectAllProducts();

        [OperationContract]
        void UpdateProduct(ProductContract product);
    }
}
