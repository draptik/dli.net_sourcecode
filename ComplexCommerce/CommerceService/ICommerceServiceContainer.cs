using System;
namespace Ploeh.Samples.CommerceService
{
    public interface ICommerceServiceContainer
    {
        void Release(object instance);

        IProductManagementService ResolveProductManagementService();
    }
}
