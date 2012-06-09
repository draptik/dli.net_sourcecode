using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.AssortedCodeSnippets.Decorator
{
    public class OutputReverser : ISalutor
    {
        private readonly ISalutor innerComponent;

        public OutputReverser(ISalutor innerComponent)
        {
            if (innerComponent == null)
            {
                throw new ArgumentNullException("innerComponent");
            }

            this.innerComponent = innerComponent;
        }

        #region ISalutor Members

        public string Greet(string name)
        {
            var returnValue = this.innerComponent.Greet(name);
            return this.Reverse(returnValue);
        }

        #endregion

        private string Reverse(string text)
        {
            return new string(text.Reverse().ToArray());
        }
    }
}
