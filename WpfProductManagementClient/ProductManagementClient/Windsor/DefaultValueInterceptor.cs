using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Core.Interceptor;
using Ploeh.Samples.ProductManagement.PresentationLogic.Wpf;
using Castle.DynamicProxy;

namespace Ploeh.Samples.ProductManagement.WpfClient.Windsor
{
    public class DefaultValueInterceptor : IInterceptor
    {
        #region IInterceptor Members

        public void Intercept(IInvocation invocation)
        {
            if (typeof(IEnumerable<ProductViewModel>).IsAssignableFrom(invocation.Method.ReturnType))
            {
                invocation.ReturnValue = Enumerable.Empty<ProductViewModel>();
            }
            invocation.Proceed();
        }

        #endregion
    }
}
