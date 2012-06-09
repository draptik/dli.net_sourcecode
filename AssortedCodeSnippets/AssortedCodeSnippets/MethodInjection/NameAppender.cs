using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.AssortedCodeSnippets.MethodInjection
{
    public class NameAppender : IAddIn
    {
        #region IAddIn Members

        public string DoStuff(SomeValue value, ISomeContext context)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }        
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return context.Name + value.Message;
        }

        #endregion
    }
}
