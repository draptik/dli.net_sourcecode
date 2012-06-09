using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.Samples.AssortedCodeSnippets.MethodInjection;
using Xunit;

namespace Ploeh.Samples.AssortedCodeSnippetsUnitTest.MethodInjection
{
    public class AddInClientTest
    {
        [Fact]
        public void CreateWithNullAddInsWillThrow()
        {
            // Fixture setup
            IEnumerable<IAddIn> nullAddIns = null;
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() => new AddInClient(nullAddIns));
            // Teardown
        }

        [Fact]
        public void DoStuffWillThrowOnNullValue()
        {
            // Fixture setup
            var fixture = new Fixture();
            fixture.Inject(Enumerable.Empty<IAddIn>());
            var sut = fixture.CreateAnonymous<AddInClient>();
            SomeValue nullValue = null;
            // Exercise system
            Assert.Throws<ArgumentNullException>(() => 
                sut.DoStuff(nullValue));
            // Verify outcome (expected exception)
            // Teardown
        }

        [Fact]
        public void DoStuffWillInvokeAddInWithCorrectValue()
        {
            // Fixture setup
            var fixture = new Fixture();
            var value = fixture.CreateAnonymous<SomeValue>();

            var addInMock = new Mock<IAddIn>();
            addInMock.Setup(ai => ai.DoStuff(It.Is<SomeValue>(sv => sv.Message == value.Message), It.IsAny<ISomeContext>())).Returns(fixture.CreateAnonymous("Message")).Verifiable();

            fixture.Register<IEnumerable<IAddIn>>(() => new[] { addInMock.Object });

            var sut = fixture.CreateAnonymous<AddInClient>();
            // Exercise system
            sut.DoStuff(value);
            // Verify outcome
            addInMock.Verify();
            // Teardown
        }

        [Fact]
        public void DoStuffWillInvokeAddInWithContextInstance()
        {
            // Fixture setup
            var fixture = new Fixture();

            var addInMock = new Mock<IAddIn>();
            addInMock.Setup(ai => ai.DoStuff(It.IsAny<SomeValue>(), It.Is<ISomeContext>(ctx => ctx != null))).Returns(fixture.CreateAnonymous("Message")).Verifiable();

            fixture.Register<IEnumerable<IAddIn>>(() => new[] { addInMock.Object });

            var sut = fixture.CreateAnonymous<AddInClient>();
            // Exercise system
            fixture.Do((SomeValue sv) => sut.DoStuff(sv));
            // Verify outcome
            addInMock.Verify();
            // Teardown
        }

        [Fact]
        public void DoStuffWillReturnValueWithMessageConstructedByAddIns()
        {
            // Fixture setup
            var fixture = new Fixture();
            var expectedMessage = fixture.CreateAnonymous("Message");

            var addInStub1 = new Mock<IAddIn>();
            addInStub1.Setup(ai => ai.DoStuff(It.IsAny<SomeValue>(), It.IsAny<ISomeContext>())).Returns(fixture.CreateAnonymous("Message"));

            var addInStub2 = new Mock<IAddIn>();
            addInStub2.Setup(ai => ai.DoStuff(It.IsAny<SomeValue>(), It.IsAny<ISomeContext>())).Returns(expectedMessage);

            fixture.Register<IEnumerable<IAddIn>>(() => new[] { addInStub1.Object, addInStub2.Object });

            var sut = fixture.CreateAnonymous<AddInClient>();
            // Exercise system
            SomeValue result = fixture.Get((SomeValue sv) => sut.DoStuff(sv));
            // Verify outcome
            Assert.Equal<string>(expectedMessage, result.Message);
            // Teardown
        }

        [Fact]
        public void DoStuffWithNameAppenderAndReverserWillReturnCorrectResult()
        {
            // Fixture setup
            var sut = new AddInClient(new IAddIn[] { new NameAppender(), new Reverser() });

            var value = new SomeValue { Message = "Ploeh" };
            // Exercise system
            var result = sut.DoStuff(value);
            // Verify outcome
            Assert.Equal<string>("heolPtneilCnIddA", result.Message);
            // Teardown
        }
    }
}
