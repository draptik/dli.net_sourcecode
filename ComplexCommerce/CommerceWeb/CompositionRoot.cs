//using System.Configuration;
//using System.Web.Mvc;
//using Ploeh.Samples.Commerce.Data.Sql;
//using Ploeh.Samples.Commerce.Domain;
//using Ploeh.Samples.Commerce.Web.PresentationModel;

//namespace Ploeh.Samples.Commerce.Web
//{
//    public class CompositionRoot
//    {
//        private readonly IControllerFactory controllerFactory;

//        public CompositionRoot()
//        {
//            this.controllerFactory = CompositionRoot.CreateControllerFactory();
//        }

//        public IControllerFactory ControllerFactory
//        {
//            get { return this.controllerFactory; }
//        }

//        private static IControllerFactory CreateControllerFactory()
//        {
//            string connectionString =
//                ConfigurationManager.ConnectionStrings
//                ["CommerceObjectContext"].ConnectionString;

//            var productRepository =
//                new SqlProductRepository(connectionString);
//            var basketRepository =
//                new SqlBasketRepository(connectionString);
//            var discountRepository =
//                new SqlDiscountRepository(connectionString);

//            var discountPolicy = 
//                new RepositoryBasketDiscountPolicy(
//                    discountRepository);

//            var controllerFactory = new CommerceControllerFactory(
//                productRepository, basketRepository, discountPolicy);

//            return controllerFactory;
//        }
//    }
//}
