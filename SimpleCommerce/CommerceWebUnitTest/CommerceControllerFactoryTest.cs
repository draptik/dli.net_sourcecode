using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;
using System.Web.Mvc;
using Ploeh.Samples.Commerce.Web;
using Ploeh.Samples.Commerce.Web.Controllers;
using System.Web.Routing;
using Ploeh.Samples.Commerce.Domain;
using Xunit;

namespace Ploeh.Samples.Commerce.WebUnitTest
{
    public class CommerceControllerFactoryTest
    {
        [Fact]
        public void CreateWithNullRepositoryWillThrow()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new CommerceWebCustomization());
            ProductRepository nullRepository = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                fixture.Build<CommerceControllerFactory>()
                    .FromFactory(() => new CommerceControllerFactory(nullRepository))
                    .CreateAnonymous());
            // Teardown
        }

        [Fact]
        public void SutIsControllerFactory()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new CommerceWebCustomization());
            // Exercise system
            var sut = fixture.CreateAnonymous<CommerceControllerFactory>();
            // Verify outcome
            Assert.IsAssignableFrom<IControllerFactory>(sut);
            // Teardown
        }

        [Fact]
        public void CreateAccountControllerWillReturnCorrectController()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new CommerceWebCustomization());
            var sut = fixture.CreateAnonymous<CommerceControllerFactory>();
            // Exercise system
            var result = fixture.Get((RequestContext ctx) => sut.CreateController(ctx, "Account"));
            // Verify outcome
            Assert.IsAssignableFrom<AccountController>(result);
            // Teardown
        }

        [Fact]
        public void CreateHomeControllerWillReturnCorrectController()
        {
            // Fixture setup
            var fixture = new Fixture().Customize(new CommerceWebCustomization());
            Type expectedControllerType = typeof(HomeController);
            var sut = fixture.CreateAnonymous<CommerceControllerFactory>();
            // Exercise system
            var result = fixture.Get((RequestContext ctx) => sut.CreateController(ctx, "Home"));
            // Verify outcome
            Assert.IsAssignableFrom<HomeController>(result);
            // Teardown
        }
    }
}
