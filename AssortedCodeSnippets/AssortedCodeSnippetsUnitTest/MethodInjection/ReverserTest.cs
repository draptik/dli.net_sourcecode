using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.Samples.AssortedCodeSnippets.MethodInjection;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.Samples.AssortedCodeSnippetsUnitTest.MethodInjection
{
    public class ReverserTest
    {
        [Theory, AutoMoqData]
        public void SutIsAddIn(Reverser sut)
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.IsAssignableFrom<IAddIn>(sut);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void DoStuffWillReverseMessage(Reverser sut, SomeValue value)
        {
            // Fixture setup
            var expectedResult = new string(value.Message.Reverse().ToArray());
            // Exercise system
            var result = sut.DoStuff(value, null);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }
    }
}
