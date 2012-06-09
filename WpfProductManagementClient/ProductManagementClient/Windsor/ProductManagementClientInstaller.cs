using System;
using System.Windows;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Ploeh.Samples.ProductManagement.PresentationLogic.Wpf;
using Ploeh.Samples.ProductManagement.WcfAgent;
using Ploeh.Samples.ProductManagement.WcfAgent.WcfClient;
using Castle.MicroKernel.SubSystems.Configuration;

namespace Ploeh.Samples.ProductManagement.WpfClient.Windsor
{
    public class ProductManagementClientInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component
                .For<IProductChannelFactory>()
                .ImplementedBy<ProductChannelFactory>());
            container.Register(Component
                .For<IClientContractMapper>()
                .ImplementedBy<ClientContractMapper>());
            container.Register(Component
                .For<IProductManagementAgent>()
                .ImplementedBy<WcfProductManagementAgent>());

            container.Register(Component
                .For<IMainWindowViewModelFactory>()
                .ImplementedBy<MainWindowViewModelFactory>());

            container.Register(Component
                .For<Window>()
                .ImplementedBy<MainWindow>());
            container.Register(Component
                .For<IWindow>()
                .ImplementedBy<MainWindowAdapter>()
                .LifeStyle.Transient);


            container.Register(Component.For<ErrorHandlingInterceptor>());
            container.Register(Component.For<DefaultValueInterceptor>());

            container.Register(Component
                .For<ICircuitBreaker>()
                .ImplementedBy<CircuitBreaker>()
                .DependsOn(new
                {
                    timeout = TimeSpan.FromMinutes(1) 
                }));
            container.Register(Component.For<CircuitBreakerInterceptor>());

            container.Kernel.ProxyFactory.AddInterceptorSelector(
                new ProductManagementClientInterceptorSelector());
        }

        #endregion
    }
}
