using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AopAlliance.Intercept;

namespace Ploeh.Samples.Menu.SpringNet
{
    public class SpyInterceptor : IMethodInterceptor
    {
        private readonly List<IMethodInvocation> invocations;

        public SpyInterceptor()
        {
            this.invocations = new List<IMethodInvocation>();
        }

        public IEnumerable<IMethodInvocation> Invocations
        {
            get { return this.invocations; }
        }

        #region IMethodInterceptor Members

        public object Invoke(IMethodInvocation invocation)
        {
            this.invocations.Add(invocation);
            return invocation.Proceed();
        }

        #endregion
    }
}
