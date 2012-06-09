using System.ServiceModel;

namespace Ploeh.Samples.ProductManagement.WcfAgent.WcfClient
{
    public class ProductChannelFactory : IProductChannelFactory
    {
        private readonly ChannelFactory<IProductManagementServiceChannel> channelFactory;

        public ProductChannelFactory()
        {
            this.channelFactory = new ChannelFactory<IProductManagementServiceChannel>(string.Empty);
        }

        #region IProductChannelFactory Members

        public IProductManagementServiceChannel CreateChannel()
        {
            return this.channelFactory.CreateChannel();
        }

        #endregion
    }
}
