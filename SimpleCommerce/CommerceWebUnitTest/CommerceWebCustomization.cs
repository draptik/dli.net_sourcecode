using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Ploeh.Samples.Commerce.WebUnitTest
{
    public class CommerceWebCustomization : CompositeCustomization
    {
        public CommerceWebCustomization()
            : base(new AutoMoqCustomization(), new MvcCustomization())
        {
        }
    }
}
