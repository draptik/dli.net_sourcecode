using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.CacheModel
{
    public interface ILease
    {
        bool IsExpired { get; }

        void Renew();
    }
}
