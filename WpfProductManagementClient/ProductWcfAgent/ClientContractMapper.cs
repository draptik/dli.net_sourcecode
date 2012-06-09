using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.ProductManagement.PresentationLogic.Wpf;
using Ploeh.Samples.ProductManagement.WcfAgent.WcfClient;

namespace Ploeh.Samples.ProductManagement.WcfAgent
{
    public class ClientContractMapper : IClientContractMapper
    {
        #region IClientContractMapper Members

        public MoneyViewModel Map(MoneyContract contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException("contract");
            }

            var vm = new MoneyViewModel();
            vm.Amount = contract.Amount;
            vm.CurrencyCode = contract.CurrencyCode;
            return vm;
        }

        public ProductViewModel Map(ProductContract contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException("contract");
            }

            var vm = new ProductViewModel();
            vm.Id = contract.Id;
            vm.Name = contract.Name;
            vm.UnitPrice = this.Map(contract.UnitPrice);
            return vm;
        }

        public IEnumerable<ProductViewModel> Map(IEnumerable<ProductContract> contracts)
        {
            if (contracts == null)
            {
                throw new ArgumentNullException("contracts");
            }

            foreach (var contract in contracts)
            {
                yield return this.Map(contract);
            }
        }

        public ProductContract Map(ProductEditorViewModel productEditorViewModel)
        {
            if (productEditorViewModel == null)
            {
                throw new ArgumentNullException("productEditorViewModel");
            }

            var pc = new ProductContract();
            pc.Id = productEditorViewModel.Id;
            pc.Name = productEditorViewModel.Name;
            pc.UnitPrice = new MoneyContract
            {
                Amount = decimal.Parse(productEditorViewModel.Price), 
                CurrencyCode = productEditorViewModel.Currency 
            };
            return pc;
        }

        #endregion
    }
}
