using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Commerce.Domain
{
    public interface IValuableItem<T>
    {
        Money Value { get; }

        T ConvertTo(Currency currency);
    }
}
