using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Ploeh.Samples.CommerceService
{
	public class CommerceServiceHost : ServiceHost
	{
		public CommerceServiceHost(ICommerceServiceContainer container,
			Type serviceType, params Uri[] baseAddresses)
			: base(serviceType, baseAddresses)
		{
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}

			var contracts = this.ImplementedContracts.Values;
			foreach (var c in contracts)
			{
				var instanceProvider =
					new CommerceInstanceProvider(
						container);
				c.Behaviors.Add(instanceProvider);
			}
		}
	}
}
