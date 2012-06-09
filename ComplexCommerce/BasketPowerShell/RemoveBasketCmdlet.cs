using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using Ploeh.Samples.Commerce.BasketPowerShellModel;

namespace Ploeh.Samples.Commerce.BasketPowerShell
{
    [Cmdlet(VerbsCommon.Remove, "Basket")]
    public class RemoveBasketCmdlet : Cmdlet
    {
        private readonly BasketManager basketManager;

        public RemoveBasketCmdlet()
        {
            this.basketManager = BasketContainer.ResolveManager();
        }

        [Parameter(ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        public string Owner { get; set; }

        protected override void ProcessRecord()
        {
            this.basketManager.RemoveBasket(this.Owner);
        }
    }
}
