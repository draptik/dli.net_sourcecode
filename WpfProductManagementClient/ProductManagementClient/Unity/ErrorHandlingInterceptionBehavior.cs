using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity.InterceptionExtension;
using System.Windows;
using System.ServiceModel;

namespace Ploeh.Samples.ProductManagement.WpfClient.Unity
{
    public class ErrorHandlingInterceptionBehavior :
        IInterceptionBehavior
    {
        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        public bool WillExecute
        {
            get { return true; }
        }

        public IMethodReturn Invoke(
            IMethodInvocation input,
            GetNextInterceptionBehaviorDelegate getNext)
        {
            var result = getNext()(input, getNext);
            if (result.Exception is CommunicationException
                || result.Exception is
                    InvalidOperationException)
            {
                this.AlertUser(result.Exception.Message);
                return input.CreateMethodReturn(null);
            }
            return result;
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
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}
