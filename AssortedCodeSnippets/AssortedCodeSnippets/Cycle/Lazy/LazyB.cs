using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.AssortedCodeSnippets.Cycle.Lazy
{
    public class LazyB : IB
    {
        public IB B { get; set; }

        #region IB Members

        public string AugmentHello()
        {
            return this.B.AugmentHello();
        }

        #endregion
    }
}
