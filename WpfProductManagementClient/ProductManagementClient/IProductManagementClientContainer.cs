using System;
using Ploeh.Samples.ProductManagement.PresentationLogic.Wpf;

namespace Ploeh.Samples.ProductManagement.WpfClient
{
    public interface IProductManagementClientContainer
    {
        IWindow ResolveWindow();
    }
}
