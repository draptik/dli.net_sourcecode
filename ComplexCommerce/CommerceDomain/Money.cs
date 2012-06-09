using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Commerce.Domain
{
    public class Money : IEquatable<Money>
    {
        private readonly decimal amount;
        private readonly string currencyCode;

        public Money(decimal amount, string currencyCode)
        {
            if (string.IsNullOrEmpty(currencyCode))
            {
                throw new ArgumentException("The currency cannot be null or empty.", "currencyCode");
            }

            this.amount = amount;
            this.currencyCode = currencyCode;
        }

        public decimal Amount
        {
            get { return this.amount; }
        }

        public string CurrencyCode
        {
            get { return this.currencyCode; }
        }

        public Money Add(decimal amount)
        {
            return new Money(this.Amount + amount, this.CurrencyCode);
        }

        public Money ConvertTo(Currency currency)
        {
            if (currency == null)
            {
                throw new ArgumentNullException("currency");
            }        

            var exchangeRate = 
                currency.GetExchangeRateFor(this.CurrencyCode);
            return new Money(this.Amount * exchangeRate,
                currency.Code);
        }

        public override bool Equals(object obj)
        {
            var m = obj as Money;
            if (m != null)
            {
                return this.Equals(m);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.Amount.GetHashCode() ^ this.CurrencyCode.GetHashCode();
        }

        public Money Multiply(decimal multiplier)
        {
            return new Money(this.Amount * multiplier, this.CurrencyCode);
        }

        public override string ToString()
        {
            return string.Format("{0:F} {1}", this.Amount, this.CurrencyCode);
        }

        #region IEquatable<Money> Members

        public bool Equals(Money other)
        {
            if (other == null)
            {
                return false;
            }
            return this.Amount == other.Amount && this.CurrencyCode == other.CurrencyCode;
        }

        #endregion
    }
}
