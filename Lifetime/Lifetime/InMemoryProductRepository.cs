using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Dependency.Lifetime
{
    // This is really only a fake, but if it had any real implementation, it should be thread-safe!
    public class InMemoryProductRepository : ProductRepository
    {
    }
}
