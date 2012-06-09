using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.Commerce.Domain;

namespace Ploeh.Samples.Commerce.UpdateCurrency.ApplicationServices
{
    public class CurrencyParser
    {
        private readonly CurrencyProvider currencyProvider;

        public CurrencyParser(CurrencyProvider currencyProvider)
        {
            if (currencyProvider == null)
            {
                throw new ArgumentNullException("currencyProvider");
            }

            this.currencyProvider = currencyProvider;
        }

        public ICommand Parse(IEnumerable<string> args)
        {
            if (args == null)
            {
                return new HelpCommand();
            }

            var argList = args.ToList();
            if (argList.Count < 3)
            {
                return new HelpCommand();
            }

            var destCurrencyCode = argList[0];
            var srcCurrencyCode = argList[1];
            decimal rate;
            if (!decimal.TryParse(argList[2], out rate))
            {
                return new HelpCommand();
            }

            var currency = this.currencyProvider.GetCurrency(srcCurrencyCode);

            return new CurrencyUpdateCommand(currency, destCurrencyCode, rate);
        }
    }
}
