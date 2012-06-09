using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.ProductManagement.WcfAgent.UnitTest
{
    internal static class Literals
    {
        internal static TimeSpan Minutes(this int m)
        {
            return TimeSpan.FromMinutes(m);
        }
    }
}
