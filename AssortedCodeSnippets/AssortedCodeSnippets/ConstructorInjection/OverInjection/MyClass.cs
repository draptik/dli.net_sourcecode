using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.AssortedCodeSnippets.ConstructorInjection.OverInjection
{
    public class MyClass
    {
        public MyClass(IUnitOfWorkFactory uowFactory,
            CurrencyProvider currencyProvider,
            IFooPolicy fooPolicy,
            IBarService barService,
            ICoffeeMaker coffeeMaker,
            IKitchenSink kitchenSink)
        {
        }
    }
}
