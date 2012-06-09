using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AopAlliance.Intercept;
using Ploeh.Samples.ProductManagement.PresentationLogic.Wpf;

namespace Ploeh.Samples.ProductManagement.WpfClient.Spring
{
    public class DefaultValueInterceptor : IMethodInterceptor
    {
        #region IMethodInterceptor Members

        public object Invoke(IMethodInvocation invocation)
        {
            var result = invocation.Proceed();
            if ((result == null)
                && typeof(IEnumerable<ProductViewModel>).IsAssignableFrom(invocation.Method.ReturnType))
            {
                return Enumerable.Empty<ProductViewModel>();
            }

            return result;
        }

        #endregion
    }
}
