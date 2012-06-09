using System;
using Moq;
using Ploeh.Samples.AssortedCodeSnippets.MethodInjection;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.Samples.AssortedCodeSnippetsUnitTest.MethodInjection
{
    public class NameAppenderTest
    {
        [Theory, AutoMoqData]
        public void SutIsAddIn(NameAppender sut)
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.IsAssignableFrom<IAddIn>(sut);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void DoStuffWithNullValueWillThrow(NameAppender sut, ISomeContext ctx)
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.DoStuff(null, ctx));
            // Teardown
        }

        [Theory, AutoMoqData]
        public void DoStuffWithNullContextWillThrow(NameAppender sut, SomeValue sv)
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                sut.DoStuff(sv, null));
            // Teardown
        }

        [Theory, AutoMoqData]
        public void DoStuffWillAppendContextNameToMessage(NameAppender sut, SomeValue value, string contextName, Mock<ISomeContext> contextStub)
        {
            // Fixture setup
            var expectedResult = contextName + value.Message;
            contextStub.SetupGet(ctx => ctx.Name).Returns(contextName);
            // Exercise system
            var result = sut.DoStuff(value, contextStub.Object);
            // Verify outcome
            Assert.Equal<string>(expectedResult, result);
            // Teardown
        }
    }
}
