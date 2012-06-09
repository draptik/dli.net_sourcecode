using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Xunit;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Ploeh.Samples.Lifetime.MvcApplication.UnitTest
{
    public class AutoMvcDataAttribute : AutoDataAttribute
    {
        public AutoMvcDataAttribute()
            : base(new Fixture().Customize(new MvcApplicationCustomization()))
        {
        }
    }
}
