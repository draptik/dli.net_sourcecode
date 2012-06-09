using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Ploeh.Samples.AssortedCodeSnippets.AmbientContext;
using Xunit;

namespace Ploeh.Samples.AssortedCodeSnippetsUnitTest.AmbientContext
{
    public class SomeClassTest : IDisposable
    {
        [Fact]
        public void GetMessageWithDefaultContextWillBeCorrect()
        {
            // Fixture setup
            var sut = new SomeClass();
            // Exercise system
            var result = sut.GetMessage();
            // Verify outcome
            Assert.Equal<string>("Default", result);
            // Teardown
        }

        [Fact]
        public void GetMessageWithReplacedContextWillReturnCorrectResult()
        {
            // Fixture setup
            SomeContext.Current = new MyContext();
            var sut = new SomeClass();
            // Exercise system
            var result = sut.GetMessage();
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