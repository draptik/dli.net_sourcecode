using System.Web.Profile;

namespace Ploeh.Samples.Commerce.Web.PresentationModel
{
    public class UserProfile : ProfileBase
    {
        #region IUserProfile Members

        public virtual string Currency
        {
            get { return (string)this["Currency"]; }
            set { this["Currency"] = value; }
        }

        #endregion
    }
}
