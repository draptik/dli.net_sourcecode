using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.AssortedCodeSnippets.Cycle
{
    public class C : IC
    {
        private readonly ID d;

        public C(ID d)
        {
            if (d == null)
            {
                throw new ArgumentNullException("d");
            }

            this.d = d;
        }

        #region IC Members

        public string Hello()
        {
            return "C";
        }

        #endregion
    }
}
