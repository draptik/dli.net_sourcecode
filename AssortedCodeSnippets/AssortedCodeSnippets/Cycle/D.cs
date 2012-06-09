using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.AssortedCodeSnippets.Cycle
{
    public class D : ID
    {
        private readonly IA a;

        public D(IA a)
        {
            if (a == null)
            {
                throw new ArgumentNullException("a");
            }

            this.a = a;
        }
    }
}
