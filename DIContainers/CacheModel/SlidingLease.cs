using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.CacheModel
{
    public class SlidingLease : ILease
    {
        private readonly TimeSpan timeout;
        private DateTime renewed;

        public SlidingLease(TimeSpan timeout)
        {
            this.timeout = timeout;
            this.renewed = DateTime.Now;
        }

        public TimeSpan Timeout
        {
            get { return this.timeout; }
        }

        public bool IsExpired
        {
            get { return DateTime.Now > 
                this.renewed + this.timeout; }
        }

        public void Renew()
        {
            this.renewed = DateTime.Now;
        }
    }
}
