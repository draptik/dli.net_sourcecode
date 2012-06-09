using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.AssortedCodeSnippets.Decorator
{
    public class InputReverser : ISalutor
    {
        private readonly ISalutor innerComponent;

        public InputReverser(ISalutor innerComponent)
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
            var reversed = this.Reverse(name);
            return this.innerComponent.Greet(reversed);
        }

        #endregion

        private string Reverse(string text)
        {
            return new string(text.Reverse().ToArray());
        }
    }
}
