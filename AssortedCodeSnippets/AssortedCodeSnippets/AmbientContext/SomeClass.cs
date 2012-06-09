using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.AssortedCodeSnippets.AmbientContext
{
    public partial class SomeClass
    {
        public string GetMessage()
        {
            return SomeContext.Current.SomeValue;
        }

        public string GetSomething(SomeService service,
            TimeProvider timeProvider)
        {
            return service.GetStuff("Foo", timeProvider);
        }
    }
}
