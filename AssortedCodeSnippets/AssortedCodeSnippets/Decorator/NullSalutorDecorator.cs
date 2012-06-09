using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.AssortedCodeSnippets.Decorator
{
    public class NullSalutorDecorator : ISalutor
    {
        private readonly ISalutor innerComponent;

        public NullSalutorDecorator(ISalutor innerComponent)
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
            return this.innerComponent.Greet(name);
        }

        #endregion
    }
}
