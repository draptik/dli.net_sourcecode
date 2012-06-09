using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.Threading;

namespace Ploeh.Samples.Commerce.Domain
{
    public class AuditEvent
    {
        private readonly DateTimeOffset timestamp;
        private readonly IIdentity identity;
        private readonly string name;
        private readonly object data;

        public AuditEvent(string name, object data)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }        
        
            this.timestamp = DateTimeOffset.Now;
            this.identity = Thread.CurrentPrincipal.Identity;
            this.name = name;
            this.data = data;
        }

        public object Data
        {
            get { return this.data; }
        }

        public IIdentity Identity
        {
            get { return this.identity; }
        }

        public string Name
        {
            get { return this.name; }
        }

        public DateTimeOffset Timestamp
        {
            get { return this.timestamp; }
        }
    }
}
