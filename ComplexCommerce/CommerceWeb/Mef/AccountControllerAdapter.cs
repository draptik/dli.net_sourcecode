using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ploeh.Samples.Commerce.Web.PresentationModel.Controllers;
using System.ComponentModel.Composition;

namespace Ploeh.Samples.Commerce.Web.Mef
{
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AccountControllerAdapter
    {
        [Export]
        public AccountController AccountController
        {
            get { return new AccountController(); }
        }
    }
}
