using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.Samples.ProductManagement.PresentationLogic.Wpf;
using Ploeh.Samples.ProductManagement.WcfAgent;
using Ploeh.Samples.ProductManagement.WcfAgent.WcfClient;
using System.Windows;

namespace Ploeh.Samples.ProductManagement.WpfClient
{
	public class ErrorHandlingProductManagementClientContainer : IProductManagementClientContainer
	{
		#region IProductManagementClientContainer Members

		public IWindow ResolveWindow()
		{
			IProductChannelFactory channelFactory =
				new ProductChannelFactory();
			IClientContractMapper mapper =
				new ClientContractMapper();
			IProductManagementAgent wcfAgent =
				new WcfProductManagementAgent(
					channelFactory, mapper);

			var timeout = TimeSpan.FromMinutes(1);
			ICircuitBreaker breaker = new CircuitBreaker(timeout);
			IProductManagementAgent circuitBreakerAgent =
				new CircuitBreakerProductManagementAgent(wcfAgent, breaker);

			IProductManagementAgent errorHandlingAgent =
				new ErrorHandlingProductManagementAgent(
					circuitBreakerAgent);

			IMainWindowViewModelFactory vmFactory =
				new MainWindowViewModelFactory(
					errorHandlingAgent);

			Window mainWindow = new MainWindow();
			IWindow w =
				new MainWindowAdapter(mainWindow, vmFactory);
			return w;
		}

		#endregion
	}
}
