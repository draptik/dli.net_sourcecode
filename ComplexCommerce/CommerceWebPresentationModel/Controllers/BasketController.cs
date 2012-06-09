using System;
using System.Web.Mvc;
using Ploeh.Samples.Commerce.Domain;
using Ploeh.Samples.Commerce.Web.PresentationModel.Models;

namespace Ploeh.Samples.Commerce.Web.PresentationModel.Controllers
{
    public class BasketController : Controller
    {
        private readonly IBasketService basketService;
        private readonly CurrencyProvider currencyProvider;

        public BasketController(IBasketService basketService,
            CurrencyProvider currencyProvider)
        {
            if (basketService == null)
            {
                throw new 
                    ArgumentNullException("basketService");
            }
            if (currencyProvider == null)
            {
                throw new 
                    ArgumentNullException("currencyProvider");
            }        

            this.basketService = basketService;
            this.currencyProvider = currencyProvider;
        }

        private CurrencyProfileService currencyProfileService;

        public CurrencyProfileService CurrencyProfileService
        {
            get
            {
                if (this.currencyProfileService == null)
                {
                    this.CurrencyProfileService = 
                        new DefaultCurrencyProfileService(
                            this.HttpContext);
                }
                return this.currencyProfileService;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if (this.currencyProfileService != null)
                {
                    throw new InvalidOperationException();
                }
                this.currencyProfileService = value;
            }
        }

        public ViewResult Index()
        {
            var currencyCode = 
                this.CurrencyProfileService.GetCurrencyCode();
            var currency = 
                this.currencyProvider.GetCurrency(currencyCode);

            var basket = this.basketService
                .GetBasketFor(this.User)
                .ConvertTo(currency);
            if (basket.Contents.Count == 0)
            {
                return this.View("Empty");
            }

            var vm = new BasketViewModel(basket);
            return this.View(vm);
        }

        public RedirectToRouteResult Add(int id)
        {
            this.basketService.AddToBasket(id, 1, this.User);

            return this.RedirectToAction("Index");
        }

        public RedirectToRouteResult Empty()
        {
            this.basketService.Empty(this.User);

            return this.RedirectToAction("Index");
        }
    }
}
