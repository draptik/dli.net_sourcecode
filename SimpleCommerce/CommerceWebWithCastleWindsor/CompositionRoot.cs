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

            this.controllerFactory = 
                new CommerceControllerFactory(repository);
        }

        public IControllerFactory ControllerFactory
        {
            get { return this.controllerFactory; }
        }
    }
}
