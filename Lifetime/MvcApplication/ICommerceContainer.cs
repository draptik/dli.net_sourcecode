using System;
using System.Web.Mvc;

namespace Ploeh.Samples.Lifetime.MvcApplication
{
    public interface ICommerceContainer
    {
        IController ResolveHomeController();
    }
}
