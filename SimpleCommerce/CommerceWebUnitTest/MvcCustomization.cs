using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture;
using System.Web.Mvc;
using System.Web;
using Moq;
using System.Security.Principal;

namespace Ploeh.Samples.Commerce.WebUnitTest
{
    public class MvcCustomization : ICustomization
    {
        #region ICustomization Members

        public void Customize(IFixture fixture)
        {
            fixture.Customize<ViewDataDictionary>(c =>
                c.Without(vdd => vdd.ModelMetadata));

            fixture.Customize<HttpContextBase>(ob => ob
                .FromFactory(() =>
                {
                    var contextStub = new Mock<HttpContextBase>();
                    contextStub.SetupProperty(ctx => ctx.User);
                    contextStub.Object.User = fixture.CreateAnonymous<IPrincipal>();
                    return contextStub.Object;
                })
                .OmitAutoProperties());
        }

        #endregion
    }
}
