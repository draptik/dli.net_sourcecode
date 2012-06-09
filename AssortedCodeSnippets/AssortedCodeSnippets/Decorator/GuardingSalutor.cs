using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.AssortedCodeSnippets.Decorator
{
    public class GuardingSalutor : ISalutor
    {
        private readonly ISalutor innerComponent;

        public GuardingSalutor(ISalutor innerComponent)
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
            if (name == null)
            {
                return "Hello world!";
            }

            return this.innerComponent.Greet(name);
        }

        #endregion
    }
}
