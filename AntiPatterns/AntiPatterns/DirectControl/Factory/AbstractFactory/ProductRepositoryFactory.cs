using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.DI.AntiPatterns.DirectControl.Factory.AbstractFactory
{
    public abstract class ProductRepositoryFactory
    {
        public abstract ProductRepository Create();
    }
}
