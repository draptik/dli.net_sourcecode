using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.AssortedCodeSnippets.Decorator
{
    public class Salutor : ISalutor
    {
        #region ISalutor Members

        public string Greet(string name)
        {
            return string.Format("Hello {0}!", name);
        }

        #endregion
    }
}
