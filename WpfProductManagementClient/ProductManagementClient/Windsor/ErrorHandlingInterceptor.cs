using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Core.Interceptor;
using System.Windows;
using System.ServiceModel;
using Ploeh.Samples.ProductManagement.WcfAgent;
using Castle.DynamicProxy;

namespace Ploeh.Samples.ProductManagement.WpfClient.Windsor
{
    public class ErrorHandlingInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch (CommunicationException e)
            {
                this.AlertUser(e.Message);
            }
            catch (InvalidOperationException e)
            {
                this.AlertUser(e.Message);
            }
        }

        private void AlertUser(string message)
        {
            var sb = new StringBuilder();
            sb.AppendLine("An error occurred.");
            sb.AppendLine("Your work is likely lost.");
            sb.AppendLine("Please try again later.");
            sb.AppendLine();
            sb.AppendLine(message);

            MessageBox.Show(sb.ToString(), "Error",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
