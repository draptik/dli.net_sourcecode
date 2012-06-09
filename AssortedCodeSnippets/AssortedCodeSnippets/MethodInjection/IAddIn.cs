using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.AssortedCodeSnippets.MethodInjection
{
    public interface IAddIn
    {
        string DoStuff(SomeValue value, ISomeContext context);
    }
}
