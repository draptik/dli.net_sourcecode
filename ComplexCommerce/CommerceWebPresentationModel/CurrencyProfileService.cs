
namespace Ploeh.Samples.Commerce.Web.PresentationModel
{
    public abstract class CurrencyProfileService
    {
        public abstract string GetCurrencyCode();

        public abstract void UpdateCurrencyCode(string currencyCode);
    }
}
