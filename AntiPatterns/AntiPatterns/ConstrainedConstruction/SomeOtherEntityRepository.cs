using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.DI.AntiPatterns.ConstrainedConstruction
{
    public class SomeOtherEntityRepository
    {
        public SomeOtherEntityRepository(ObjectContext ctx)
        {
        }
    }
}
