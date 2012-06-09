using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.AssortedCodeSnippets.AmbientContext
{
    public class MyContext : SomeContext
    {
        public override string SomeValue
        {
            get { return "Ploeh"; }
        }
    }
}
