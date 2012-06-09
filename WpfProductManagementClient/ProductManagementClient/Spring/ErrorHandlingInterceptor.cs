using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AopAlliance.Intercept;
using System.Windows;
using System.ServiceModel;

namespace Ploeh.Samples.ProductManagement.WpfClient.Spring
{
    public class ErrorHandlingInterceptor : IMethodInterceptor
    {
        public object Invoke(IMethodInvocation invocation)
        {
            try
            {
                return invocation.Proceed();
            }
            catch (CommunicationException e)
            {
                this.AlertUser(e.Message);
            }
            catch (InvalidOperationException e)
            {
                this.AlertUser(e.Message);
            }
            return null;
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
