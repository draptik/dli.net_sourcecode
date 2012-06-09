using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Ploeh.Samples.ProductManagement.WcfAgent;
using Ploeh.Samples.ProductManagement.WcfAgent.WcfClient;
using Ploeh.Samples.ProductManagement.PresentationLogic.Wpf;
using System.Windows;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Ploeh.Samples.ProductManagement.WpfClient.Unity
{
    public class ProductManagementClientContainerExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Container.AddNewExtension<Interception>();
            this.Container.RegisterType<ICircuitBreaker, CircuitBreaker>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(TimeSpan.FromMinutes(1)));

            this.Container.RegisterType<IProductChannelFactory, ProductChannelFactory>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<IClientContractMapper, ClientContractMapper>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<IProductManagementAgent,
                WcfProductManagementAgent>(
                new ContainerControlledLifetimeManager(),
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<DefaultValueInterceptionBehavior>(),
                new InterceptionBehavior<ErrorHandlingInterceptionBehavior>(),
                new InterceptionBehavior<CircuitBreakerInteceptionBehavior>());

            this.Container.RegisterType<IMainWindowViewModelFactory, MainWindowViewModelFactory>(new ContainerControlledLifetimeManager());

            this.Container.RegisterType<Window, MainWindow>();
            this.Container.RegisterType<IWindow, MainWindowAdapter>();
        }
    }
}
