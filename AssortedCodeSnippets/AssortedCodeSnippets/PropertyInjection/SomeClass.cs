using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.AssortedCodeSnippets.PropertyInjection
{
    public partial class SomeClass
    {
        public ISomeInterface Dependency { get; set; }
    }

    public partial class SomeClass
    {
        public string DoSomething(string message)
        {
            return this.Dependency.DoStuff(message);
        }
    }
}
