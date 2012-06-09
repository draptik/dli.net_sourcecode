using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ploeh.AutoFixture.Xunit;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Ploeh.Samples.ProductManagement.PresentationLogic.Wpf.UnitTest
{
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute()
            : base(new AutoMoqFixture())
        {
        }
    }
}
