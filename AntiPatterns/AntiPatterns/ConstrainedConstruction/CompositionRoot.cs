using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.DI.AntiPatterns.ConstrainedConstruction
{
    public class CompositionRoot
    {
        public void Compose()
        {
            var type = typeof(SomeImplementation);
            var dep = (ISomeDependency)Activator.CreateInstance(type);
        }
    }
}
