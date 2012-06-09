using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.AssortedCodeSnippets.Cycle.Loose
{
    public class B : IB
    {
        public IC C { get; set; }

        #region IB Members

        public string AugmentHello()
        {
            return "LooseB:" + this.C.Hello();
        }

        #endregion
    }
}
