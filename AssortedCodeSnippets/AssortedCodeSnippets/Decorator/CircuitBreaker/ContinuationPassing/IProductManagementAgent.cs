using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.AssortedCodeSnippets.Decorator.CircuitBreaker.ContinuationPassing
{
    public interface IProductManagementAgent
    {
        void InsertProduct(ProductEditorViewModel product);
    }
}
