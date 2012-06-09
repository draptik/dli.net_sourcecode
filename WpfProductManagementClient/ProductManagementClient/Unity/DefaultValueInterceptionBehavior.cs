using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity.InterceptionExtension;
using System.Reflection;
using Ploeh.Samples.ProductManagement.PresentationLogic.Wpf;

namespace Ploeh.Samples.ProductManagement.WpfClient.Unity
{
    public class DefaultValueInterceptionBehavior : IInterceptionBehavior
    {
        #region IInterceptionBehavior Members

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            var result = getNext()(input, getNext);
            var mi = input.MethodBase as MethodInfo;
            if ((mi != null)
                && (typeof(IEnumerable<ProductViewModel>).IsAssignableFrom(mi.ReturnType))
                && (result.ReturnValue == null))
            {
                result.ReturnValue = Enumerable.Empty<ProductViewModel>();
            }
            return result;
        }

        public bool WillExecute
        {
            get { return true; }
        }

        #endregion
    }
}
