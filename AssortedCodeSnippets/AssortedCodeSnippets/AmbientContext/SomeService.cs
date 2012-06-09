using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.AssortedCodeSnippets.AmbientContext
{
    public class SomeService
    {
        public string GetStuff(string s, TimeProvider timeProvider)
        {
            return this.Stuff(s);
        }

        private string Stuff(string s)
        {
            return s;
        }
    }
}
