using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture;
using Ploeh.Samples.Commerce.Domain;
using Moq;
using System.Security.Principal;
using Ploeh.AutoFixture.AutoMoq;

namespace CommerceDomainUnitTest
{
    internal class RepositoryFixture : Fixture
    {
        public RepositoryFixture()
        {
            this.Customize(new AutoMoqCustomization());
        }
    }
}
