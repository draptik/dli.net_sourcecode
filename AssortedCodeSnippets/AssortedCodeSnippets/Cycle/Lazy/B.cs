using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.AssortedCodeSnippets.Cycle.Lazy
{
    public class B : IB
    {
        private readonly IC c;

        public B(IC c)
        {
            if (c == null)
            {
                throw new ArgumentNullException("c");
            }

            this.c = c;
        }

        #region IB Members

        public string AugmentHello()
        {
            return "InvariantB:" + this.c.Hello();
        }

        #endregion
    }
}
