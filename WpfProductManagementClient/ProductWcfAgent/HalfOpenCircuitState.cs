using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.ProductManagement.WcfAgent
{
    public class HalfOpenCircuitState : ICircuitState
    {
        private readonly TimeSpan timeout;
        private bool tripped;
        private bool succeeded;

        public HalfOpenCircuitState(TimeSpan timeout)
        {
            this.timeout = timeout;
        }

        #region ICircuitState Members

        public void Guard()
        {
        }

        public ICircuitState NextState()
        {
            if (this.succeeded)
            {
                return new ClosedCircuitState(this.timeout);
            }
            if (this.tripped)
            {
                return new OpenCircuitState(this.timeout);
            }
            return this;
        }

        public void Succeed()
        {
            this.succeeded = true;
        }

        public void Trip(Exception e)
        {
            this.tripped = true;
        }

        #endregion
    }
}
