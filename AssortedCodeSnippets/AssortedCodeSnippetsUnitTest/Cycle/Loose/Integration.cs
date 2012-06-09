using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Ploeh.Samples.AssortedCodeSnippets.Cycle.Loose;
using Ploeh.Samples.AssortedCodeSnippets.Cycle;

namespace Ploeh.Samples.AssortedCodeSnippetsUnitTest.Cycle.Loose
{
    public class Integration
    {
        [Fact]
        public void CreateCycle()
        {
            var b = new B();
            var a = new A(b);
            b.C = new C(new D(a));

            Assert.Equal("LooseB:C", a.Foo());
        }
    }
}
