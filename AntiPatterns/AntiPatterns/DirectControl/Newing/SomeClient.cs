using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.DI.AntiPatterns.DirectControl.Newing
{
    public class SomeClient
    {
        public string DoSomething(string message)
        {
            ISomeInterface dependency = new SomeImplementation();
            return dependency.DoStuff(message);
        }
    }
}
