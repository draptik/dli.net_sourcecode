using System;
using Ploeh.Samples.ProductManagement.PresentationLogic.Wpf;

namespace Ploeh.Samples.ProductManagement.WpfClient
{
    public class MainWindowViewModelFactory : IMainWindowViewModelFactory
    {
        private readonly IProductManagementAgent agent;

        public MainWindowViewModelFactory(IProductManagementAgent agent)
        {
            if (agent == null)
            {
                throw new ArgumentNullException("agent");
            }

            this.agent = agent;
        }

        #region IViewModelFactory Members

        public MainWindowViewModel Create(IWindow window)
        {
            if (window == null)
            {
                throw new ArgumentNullException("window");
            }
        
            return new MainWindowViewModel(this.agent, window);
        }

        #endregion
    }
}
