using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Ploeh.Samples.AssortedCodeSnippets.AmbientContext;
using Xunit;

namespace Ploeh.Samples.AssortedCodeSnippetsUnitTest.AmbientContext
{
    public class SomeContextTest : IDisposable
    {
        [Fact]
        public void DefaultContextReturnsCorrectValue()
        {
            // Fixture setup
            // Exercise system
            var result = SomeContext.Current.SomeValue;
            // Verify outcome
            Assert.Equal<string>("Default", result);
            // Teardown
        }

        [Fact]
        public void ReplacedContextReturnsCorrectValue()
        {
            // Fixture setup
            SomeContext.Current = new MyContext();
            // Exercise system
            var result = SomeContext.Current.SomeValue;
            // Verify outcome
            Assert.Equal<string>("Ploeh", result);
            // Teardown
        }

        #region IDisposable Members

        public void Dispose()
        {
            SomeContext.Current = SomeContext.Default;
        }

        #endregion
    }
}
