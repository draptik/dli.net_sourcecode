using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.AssortedCodeSnippets.Cycle
{
    public class A : IA
    {
        private readonly IB b;

        public A(IB b)
        {
            if (b == null)
            {
                throw new ArgumentNullException("b");
            }

            this.b = b;
        }

        #region IA Members

        public string Foo()
        {
            return this.b.AugmentHello();
        }

        #endregion
    }
}
