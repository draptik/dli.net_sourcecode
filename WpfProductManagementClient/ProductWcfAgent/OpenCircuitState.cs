using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.ProductManagement.WcfAgent
{
    public class OpenCircuitState : ICircuitState
    {
        private readonly TimeSpan timeout;
        private readonly DateTime closedAt;

        public OpenCircuitState(TimeSpan timeout)
        {
            this.timeout = timeout;
            this.closedAt = TimeProvider.Current.UtcNow;
        }

        #region ICircuitState Members

        public void Guard()
        {
            throw new InvalidOperationException("The circuit is currently open.");
        }

        public ICircuitState NextState()
        {
            if (TimeProvider.Current.UtcNow - this.closedAt >= this.timeout)
            {
                return new HalfOpenCircuitState(this.timeout);
            }
            return this;
        }

        public void Succeed()
        {
        }

        public void Trip(Exception e)
        {
        }

        #endregion
    }
}
