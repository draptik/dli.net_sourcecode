using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Commerce.Domain
{
    public class DefaultTimeProvider : TimeProvider
    {
        public override DateTime UtcNow
        {
            get { return DateTime.UtcNow; }
        }
    }
}
