using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.Commerce.Domain
{
    public interface IAuditor
    {
        void Record(AuditEvent @event);
    }
}
