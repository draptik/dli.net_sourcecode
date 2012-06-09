using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.DI.AntiPatterns.DirectControl
{
    public interface ISomeInterface
    {
        string DoStuff(string message);
    }
}
