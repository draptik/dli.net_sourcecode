using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.ComponentModel.Composition;

namespace Ploeh.Samples.Commerce.Web.Mef
{
    public class ConnectionStringAdapter
    {
        [Export("connectionString")]
        public string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["CommerceObjectContext"].ConnectionString;
            }
        }
    }
}
