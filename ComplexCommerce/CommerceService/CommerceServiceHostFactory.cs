using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Activation;
using System.ServiceModel;

namespace Ploeh.Samples.CommerceService
{
	public class CommerceServiceHostFactory : ServiceHostFactory
	{
		private readonly ICommerceServiceContainer container;

		public CommerceServiceHostFactory()
		{
			this.container =
				new ReleasingCommerceServiceContainer();
		}

		protected override ServiceHost CreateServiceHost(
			Type serviceType, Uri[] baseAddresses)
		{
			if (serviceType == typeof(ProductManagementService))
			{
				return new CommerceServiceHost(
					this.container,
					serviceType, baseAddresses);
			}
			return base.CreateServiceHost(serviceType, baseAddresses);
		}
	}
}
