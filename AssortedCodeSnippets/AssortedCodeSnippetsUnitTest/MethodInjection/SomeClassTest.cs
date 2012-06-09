using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Ploeh.Samples.AssortedCodeSnippets.MethodInjection;
using Ploeh.AutoFixture;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.Samples.AssortedCodeSnippetsUnitTest.MethodInjection
{
    public class SomeClassTest
    {
        [Theory, AutoMoqData]
        public void DoStuffWithNullContextWillThrow(SomeClass sut, SomeValue sv)
        {
            // Fixture setup
            ISomeContext nullContext = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.DoStuff(sv, nullContext));
            // Teardown
        }

        [Theory, AutoMoqData]
        public void DoStuffWillReturnContextName(SomeClass sut, string expectedName, Mock<ISomeContext> contextStub, SomeValue sv)
        {
            // Fixture setup
            contextStub.SetupGet(ctx => ctx.Name).Returns(expectedName);
            // Exercise system
            var result = sut.DoStuff(sv, contextStub.Object);
            // Verify outcome
            Assert.Equal(expectedName, result);
            // Teardown
        }
    }
}
