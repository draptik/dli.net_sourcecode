using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using Ploeh.Samples.Commerce.BasketPowerShellModel;

namespace Ploeh.Samples.Commerce.BasketPowerShell
{
    [Cmdlet(VerbsCommon.Get, "Basket")]
    public class GetBasketCmdlet : Cmdlet
    {
        private readonly BasketManager basketManager;

        public GetBasketCmdlet()
        {
            this.basketManager = 
                BasketContainer.ResolveManager();
        }

        protected override void ProcessRecord()
        {
            var baskets = 
                this.basketManager.GetAllBaskets();
            this.WriteObject(baskets, true);
        }
    }
}
