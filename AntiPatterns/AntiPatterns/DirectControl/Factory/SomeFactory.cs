using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.DI.AntiPatterns.DirectControl.Factory
{
    public static class SomeFactory
    {
        public static ISomeInterface Create()
        {
            return new SomeImplementation();
        }
    }
}
