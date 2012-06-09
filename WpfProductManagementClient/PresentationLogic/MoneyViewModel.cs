using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.ProductManagement.PresentationLogic.Wpf
{
    public class MoneyViewModel
    {
        public decimal Amount { get; set; }

        public string CurrencyCode { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1:F}", this.CurrencyCode, this.Amount);
        }
    }
}
