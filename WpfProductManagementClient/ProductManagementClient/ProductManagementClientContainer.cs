using System.Windows;
using Ploeh.Samples.ProductManagement.PresentationLogic.Wpf;
using Ploeh.Samples.ProductManagement.WcfAgent;
using Ploeh.Samples.ProductManagement.WcfAgent.WcfClient;

namespace Ploeh.Samples.ProductManagement.WpfClient
{
    public class ProductManagementClientContainer : IProductManagementClientContainer
    {
        public IWindow ResolveWindow()
        {
            IProductChannelFactory channelFactory =
                new ProductChannelFactory();
            IClientContractMapper mapper =
                new ClientContractMapper();
            IProductManagementAgent agent = 
                new WcfProductManagementAgent(
                    channelFactory, mapper);

            IMainWindowViewModelFactory vmFactory = 
                new MainWindowViewModelFactory(agent);

            Window mainWindow = new MainWindow();
            IWindow w = 
                new MainWindowAdapter(mainWindow, vmFactory);
            return w;
        }
    }
}
