using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.AssortedCodeSnippets.MethodInjection
{
    public class Reverser : IAddIn
    {
        #region IAddIn Members

        public string DoStuff(SomeValue value, ISomeContext context)
        {
            return new string(value.Message.Reverse().ToArray());
        }

        #endregion
    }
}
