using System.Configuration;
using Ploeh.Samples.Commerce.Data.Sql;
using Ploeh.Samples.Commerce.UpdateCurrency.ApplicationServices;
using Ploeh.Samples.Commerce.Domain;

namespace Ploeh.Samples.Commerce.UpdateCurrency.CommandLine
{
    public class CurrencyContainer
    {
        public CurrencyParser ResolveCurrencyParser()
        {
            string connectionString =
                ConfigurationManager.ConnectionStrings
                ["CommerceObjectContext"].ConnectionString;

            CurrencyProvider provider = 
                new SqlCurrencyProvider(connectionString);
            return new CurrencyParser(provider);
        }
    }
}
