using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.ProductManagement.WcfAgent.WcfClient;
using Ploeh.Samples.ProductManagement.PresentationLogic.Wpf;

namespace Ploeh.Samples.ProductManagement.WcfAgent
{
    public interface IClientContractMapper
    {
        MoneyViewModel Map(MoneyContract contract);

        ProductViewModel Map(ProductContract contract);

        IEnumerable<ProductViewModel> Map(IEnumerable<ProductContract> contracts);

        ProductContract Map(ProductEditorViewModel productEditorViewModel);
    }
}
