using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ploeh.Samples.Lifetime.MvcApplication
{
	public class DiscountRepositoryLifestyleModule : IHttpModule
	{
		public void Init(HttpApplication context)
		{
			context.EndRequest += this.OnEndRequest;
		}

		public void Dispose() { }

		private void OnEndRequest(object sender, EventArgs e)
		{
			var repository = HttpContext.Current
				.Items["DiscountRepository"];
			if (repository == null)
			{
				return;
			}

			var disposable = repository as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}

			HttpContext.Current
				.Items.Remove("DiscountRepository");
		}
	}
}
