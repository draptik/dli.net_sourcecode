using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.AssortedCodeSnippets.ConstructorInjection.AggregateService
{
    public interface INotificationService
    {
        void OrderAdded(Order order);
    }
}
