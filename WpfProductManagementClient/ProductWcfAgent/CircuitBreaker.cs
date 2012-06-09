using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.ProductManagement.WcfAgent
{
    // This implementation is not thread-safe, but really ought to be in a production scenario.
    public class CircuitBreaker : ICircuitBreaker
    {
        private ICircuitState state;

        public CircuitBreaker(TimeSpan timeout)
        {
            this.state = new ClosedCircuitState(timeout);
        }

        public ICircuitState State
        {
            get { return this.state; }
        }

        #region ICircuitBreaker Members

        public void Guard()
        {
            this.state = this.state.NextState();
            this.state.Guard();
            this.state = this.state.NextState();
        }

        public void Trip(Exception e)
        {
            this.state = this.state.NextState();
            this.state.Trip(e);
            this.state = this.state.NextState();
        }

        public void Succeed()
        {
            this.state = this.state.NextState();
            this.state.Succeed();
            this.state = this.state.NextState();
        }

        #endregion
    }
}
