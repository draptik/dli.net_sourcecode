using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.Composition.Hosting;

namespace Ploeh.Samples.Commerce.Web.Mef
{
    public class CommerceCatalog : AssemblyCatalog
    {
        public CommerceCatalog()
            : base(typeof(CommerceCatalog).Assembly)
        {
        }
    }
}
