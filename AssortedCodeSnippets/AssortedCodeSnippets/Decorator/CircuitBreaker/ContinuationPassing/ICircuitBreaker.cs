using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.AssortedCodeSnippets.Decorator.CircuitBreaker.ContinuationPassing
{
    public interface ICircuitBreaker
    {
        void Execute(Action action);

        T Execute<T>(Func<T> action);
    }
}
