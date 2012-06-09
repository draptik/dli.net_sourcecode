using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.AssortedCodeSnippets.ConstructorInjection
{
    public class SomeClass
    {
        private readonly ISomeInterface dependency;

        public SomeClass(): this(new LocalDefault())
        {
        }

        public SomeClass(ISomeInterface dependency)
        {
            if (dependency == null)
            {
                throw new ArgumentNullException("dependency");
            }

            this.dependency = dependency;
        }
    }
}
