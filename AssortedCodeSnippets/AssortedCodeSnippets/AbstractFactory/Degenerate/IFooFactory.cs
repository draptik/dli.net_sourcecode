using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.AssortedCodeSnippets.AbstractFactory.Degenerate
{
    public interface IFooFactory
    {
        IFoo Create();
    }
}
