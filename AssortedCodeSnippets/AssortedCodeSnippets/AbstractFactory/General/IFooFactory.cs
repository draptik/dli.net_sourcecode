using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.AssortedCodeSnippets.AbstractFactory.General
{
    public interface IFooFactory
    {
        IFoo Create(Bar bar);
    }
}
