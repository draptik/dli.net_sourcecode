using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Ploeh.Samples.ProductManagement.PresentationLogic.Wpf;
using Microsoft.Practices.Unity;
using Spring.Context.Support;

namespace Ploeh.Samples.ProductManagement.WpfClient
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var container =
                new ProductManagementClientContainer();
            container.ResolveWindow().Show();

            // Replace the above two lines with the following line to see Windsor Interceptors in action instead of Poor Man's DI.
            //new Castle.Windsor.WindsorContainer().Install(new Ploeh.Samples.ProductManagement.WpfClient.Windsor.ProductManagementClientInstaller()).Resolve<IWindow>().Show();

            // Replace the above two lines with the following line to see Unity Interceptors in action instead of Poor Man's DI.
            //new UnityContainer().AddNewExtension<Ploeh.Samples.ProductManagement.WpfClient.Unity.ProductManagementClientContainerExtension>().Resolve<IWindow>().Show();

            // Replace the above two lines with the following line to see Spring.NET Interceptors in action instead of Poor Man's DI.
            //new XmlApplicationContext("config://spring/objects").GetObjectsOfType(typeof(IWindow)).Values.Cast<IWindow>().Single().Show();
        }
    }
}
