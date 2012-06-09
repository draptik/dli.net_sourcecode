using System;
using System.Web.Mvc;
using Ploeh.Samples.Commerce.Domain;
using Ploeh.Samples.Commerce.Web.PresentationModel.Models;

namespace Ploeh.Samples.Commerce.Web.PresentationModel.Controllers
{
    [HandleError]
    public partial class HomeController : Controller
    {
        private readonly ProductRepository repository;
        private readonly CurrencyProvider currencyProvider;
        private CurrencyProfileService currencyProfileService;
        private bool currencyProfileServiceLocked;

        public HomeController(ProductRepository repository, CurrencyProvider currencyProvider)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            if (currencyProvider == null)
            {
                throw new ArgumentNullException("currencyProvider");
            }

            this.repository = repository;
            this.currencyProvider = currencyProvider;
        }

        public CurrencyProfileService CurrencyProfileService
        {
            get 
            {
                if (this.currencyProfileService == null)
                {
                    this.CurrencyProfileService = new DefaultCurrencyProfileService(this.HttpContext);
                }
                return this.currencyProfileService;
            }
            set 
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if (this.currencyProfileServiceLocked)
                {
                    throw new InvalidOperationException();
                }
                this.currencyProfileService = value;
                this.currencyProfileServiceLocked = true;
            }
        }

        public ViewResult About()
        {
            return View();
        }

        public ViewResult Index()
        {
            var currencyCode = this.CurrencyProfileService.GetCurrencyCode();
            var currency = this.currencyProvider.GetCurrency(currencyCode);

            var productService =
                new ProductService(this.repository);

            var vm = new FeaturedProductsViewModel();

            var products =
                productService.GetFeaturedProducts(this.User);

            foreach (var product in products)
            {
                var productVM = new ProductViewModel(product.ConvertTo(currency));
                vm.Products.Add(productVM);
            }

            return this.View(vm);
        }

        public RedirectToRouteResult SetCurrency(string id)
        {
            this.CurrencyProfileService.UpdateCurrencyCode(id);

            return this.RedirectToAction("Index");
        }
    }
}
