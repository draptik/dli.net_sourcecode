using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.ProductManagement.PresentationLogic.Wpf;
using System.ServiceModel;
using System.Windows;

namespace Ploeh.Samples.ProductManagement.WpfClient
{
    public class ErrorHandlingProductManagementAgent : IProductManagementAgent
    {
        private readonly IProductManagementAgent innerAgent;

        public ErrorHandlingProductManagementAgent(IProductManagementAgent agent)
        {
            if (agent == null)
            {
                throw new ArgumentNullException("agent");
            }

            this.innerAgent = agent;
        }

        #region IProductManagementAgent Members

        public void DeleteProduct(int productId)
        {
            try
            {
                this.innerAgent.DeleteProduct(productId);
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

        public void InsertProduct(ProductEditorViewModel product)
        {
            try
            {
                this.innerAgent.InsertProduct(product);
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

        public IEnumerable<ProductViewModel> SelectAllProducts()
        {
            try
            {
                return this.innerAgent.SelectAllProducts();
            }
            catch (CommunicationException e)
            {
                this.AlertUser(e.Message);
                return Enumerable.Empty<ProductViewModel>();
            }
            catch (InvalidOperationException e)
            {
                this.AlertUser(e.Message);
                return Enumerable.Empty<ProductViewModel>();
            }
        }

        public void UpdateProduct(ProductEditorViewModel product)
        {
            try
            {
                this.innerAgent.UpdateProduct(product);
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

        #endregion

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
