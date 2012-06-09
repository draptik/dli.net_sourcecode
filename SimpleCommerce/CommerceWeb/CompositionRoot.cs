using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using Ploeh.Samples.Commerce.Domain;

namespace Ploeh.Samples.Commerce.Web
{
    public class CompositionRoot
    {
        private readonly IControllerFactory controllerFactory;

        public CompositionRoot()
        {
            this.controllerFactory = CompositionRoot.CreateControllerFactory();
        }

        public IControllerFactory ControllerFactory
        {
            get { return this.controllerFactory; }
        }

        private static IControllerFactory CreateControllerFactory()
        {
            string connectionString =
                ConfigurationManager.ConnectionStrings
                ["CommerceObjectContext"].ConnectionString;

            string productRepositoryTypeName =
                ConfigurationManager.AppSettings
                ["ProductRepositoryType"];
            var productRepositoryType =
                Type.GetType(productRepositoryTypeName, true);
            var repository =
                (ProductRepository)Activator.CreateInstance(
                productRepositoryType, connectionString);

            var controllerFactory =
                new CommerceControllerFactory(repository);

            return controllerFactory;
        }
    }
}
