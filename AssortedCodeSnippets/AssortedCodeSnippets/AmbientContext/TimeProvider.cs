using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.AssortedCodeSnippets.AmbientContext
{
    public abstract class TimeProvider
    {
        public abstract DateTime UtcNow { get; }
    }
}
