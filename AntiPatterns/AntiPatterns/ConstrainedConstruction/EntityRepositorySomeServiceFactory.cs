using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.DI.AntiPatterns.ConstrainedConstruction
{
    public class EntityRepositorySomeServiceFactory : ISomeServiceFactory
    {
        #region ISomeServiceFactory Members

        public ISomeService CreateService()
        {


            throw new NotImplementedException();
        }

        #endregion
    }
}
