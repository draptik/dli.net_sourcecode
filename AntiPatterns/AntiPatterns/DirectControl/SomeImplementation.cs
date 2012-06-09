using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.DI.AntiPatterns.DirectControl
{
    public class SomeImplementation : ISomeInterface
    {
        #region ISomeInterface Members

        public string DoStuff(string message)
        {
            return new string(message.Reverse().ToArray());
        }

        #endregion
    }
}
