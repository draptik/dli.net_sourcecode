using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.Samples.AssortedCodeSnippets.Cycle;
using Ploeh.Samples.AssortedCodeSnippets.Cycle.Lazy;

namespace Ploeh.Samples.AssortedCodeSnippetsUnitTest.Cycle.Lazy
{
    public class Integration
    {
        [Fact]
        public void CreateCycle()
        {
            var lb = new LazyB();
            var a = new A(lb);
            lb.B = new B(new C(new D(a)));

            Assert.Equal("InvariantB:C", a.Foo());
        }
    }
}
