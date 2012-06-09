using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.DI.AntiPatterns.DirectControl.Factory
{
    public class SomeClient
    {
        public string DoSomething(string message)
        {
            var dependency = SomeFactory.Create();
            return dependency.DoStuff(message);
        }
    }
}
