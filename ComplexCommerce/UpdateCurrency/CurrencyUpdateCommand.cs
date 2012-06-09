using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.Commerce.Domain;

namespace Ploeh.Samples.Commerce.UpdateCurrency.CommandLine
{
    public class CurrencyUpdateCommand : ICommand
    {
        private readonly Currency currency;
        private readonly string destinationCode;
        private readonly decimal rate;

        public CurrencyUpdateCommand(Currency currency, string destinationCode, decimal rate)
        {
            if (currency == null)
            {
                throw new ArgumentNullException("currency");
            }
            if (destinationCode == null)
            {
                throw new ArgumentNullException("destinationCode");
            }        

            this.currency = currency;
            this.destinationCode = destinationCode;
            this.rate = rate;
        }

        public Currency Currency
        {
            get { return this.currency; }
        }

        public string DestinationCode
        {
            get { return this.destinationCode; }
        }

        public decimal Rate
        {
            get { return this.rate; }
        }

        #region ICommand Members

        public void Execute()
        {
            this.Currency.UpdateExchangeRate(this.DestinationCode, this.Rate);
            Console.WriteLine("Updated: 1 {0} in {1} = {2}.", this.DestinationCode, this.Currency.Code, this.Rate);
        }

        #endregion
    }
}
