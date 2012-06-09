using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Ploeh.Samples.ProductManagement.PresentationLogic.Wpf;
using Castle.MicroKernel.Proxy;
using Castle.Core;

namespace Ploeh.Samples.ProductManagement.WpfClient.Windsor
{
    public class ProductManagementClientInterceptorSelector :
        IModelInterceptorsSelector
    {
        public bool HasInterceptors(ComponentModel model)
        {
            return typeof(IProductManagementAgent)
                .IsAssignableFrom(model.Service);
        }

        public InterceptorReference[] 
            SelectInterceptors(ComponentModel model, 
                InterceptorReference[] interceptors)
        {
            return new[] 
            {
                InterceptorReference
                    .ForType<DefaultValueInterceptor>(),
                InterceptorReference
                    .ForType<ErrorHandlingInterceptor>(),
                InterceptorReference
                    .ForType<CircuitBreakerInterceptor>()
            };
        }
    }
}
