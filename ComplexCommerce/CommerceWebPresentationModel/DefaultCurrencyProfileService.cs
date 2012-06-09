using System;
using System.Web;

namespace Ploeh.Samples.Commerce.Web.PresentationModel
{
    public class DefaultCurrencyProfileService : CurrencyProfileService
    {
        private readonly HttpContextBase httpContext;

        public DefaultCurrencyProfileService(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            this.httpContext = httpContext;
        }

        public override string GetCurrencyCode()
        {
            UserProfile userProfile = this.httpContext.Profile as UserProfile;
            if (userProfile == null)
            {
                return "DKK";
            }
            if (string.IsNullOrEmpty(userProfile.Currency))
            {
                return "DKK";
            }
            return userProfile.Currency;
        }

        public override void UpdateCurrencyCode(string currencyCode)
        {
            UserProfile userProfile = this.httpContext.Profile as UserProfile;
            if (userProfile == null)
            {
                throw new InvalidOperationException("The Profile is not a UserProfile.");
            }
            userProfile.Currency = currencyCode;
            userProfile.Save();
        }
    }
}
