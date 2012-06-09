using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.AssortedCodeSnippets.MethodInjection
{
    public class SomeClass : IAddIn
    {
        public void DoStuff(ISomeInterface dependency)
        {
        }

        #region IAddIn Members

        public string DoStuff(SomeValue value, ISomeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return context.Name;
        }

        #endregion
    }
}
