using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Ploeh.Samples.Lifetime.MvcApplication.UnitTest
{
    public class MvcApplicationCustomization : CompositeCustomization
    {
        public MvcApplicationCustomization()
            : base(new AutoMoqCustomization(), new MvcCustomization())
        {
        }
    }
}
