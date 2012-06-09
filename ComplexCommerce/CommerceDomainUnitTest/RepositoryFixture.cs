using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture;
using Ploeh.Samples.Commerce.Domain;
using Moq;
using System.Security.Principal;

namespace Ploeh.Samples.Commerce.DomainUnitTest
{
    internal class RepositoryFixture : Fixture
    {
        public RepositoryFixture()
        {
            this.Register(() => new Mock<BasketRepository>().Object);
            this.Register(() => new Mock<DiscountRepository>().Object);
            this.Register(() => new Mock<ProductRepository>().Object);
            this.Register(() => new Mock<IPrincipal>().Object);
            this.Register(() => 
                {
                    var discountPolicyStub = new Mock<BasketDiscountPolicy>();
                    discountPolicyStub.Setup(dp => dp.Apply(It.IsAny<Basket>())).Returns((Basket b) => b);
                    return discountPolicyStub.Object;
                });
        }
    }
}
