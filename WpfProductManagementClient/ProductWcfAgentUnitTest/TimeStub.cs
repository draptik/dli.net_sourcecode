using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;

namespace Ploeh.Samples.ProductManagement.WcfAgent.UnitTest
{
    internal static class TimeStub
    {
        internal static void Freeze(this DateTime dt)
        {
            var timeProviderStub = new Mock<TimeProvider>();
            timeProviderStub.SetupGet(tp => tp.UtcNow).Returns(dt);
            TimeProvider.Current = timeProviderStub.Object;
        }

        internal static void Pass(this TimeSpan ts)
        {
            var previousTime = TimeProvider.Current.UtcNow;
            (previousTime + ts).Freeze();
        }
    }
}
