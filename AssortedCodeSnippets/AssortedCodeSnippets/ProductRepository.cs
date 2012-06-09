using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.AssortedCodeSnippets
{
    public abstract class ProductRepository
    {
        public abstract void InsertProduct(Product product);
    }
}
