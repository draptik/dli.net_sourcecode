using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Moq;
using Ploeh.Samples.HelloDI.CommandLine;
using System.Security.Principal;
using System.Threading;

namespace Ploeh.Samples.HelloDI.UnitTest
{
    public class SecureMessageWriterTest : IDisposable
    {
        private readonly IPrincipal originalPrincipal;

        public SecureMessageWriterTest()
        {
            this.originalPrincipal = Thread.CurrentPrincipal;
        }

        [Fact]
        public void SutIsMessageWriter()
        {
            // Fixture setup
            var dummyMessageWriter = new Mock<IMessageWriter>().Object;
            // Exercise system
            var sut = new SecureMessageWriter(dummyMessageWriter);
            // Verify outcome
            Assert.IsAssignableFrom<IMessageWriter>(sut);
            // Teardown
        }

        [Fact]
        public void InitializeWithNullWriterThrows()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new SecureMessageWriter(null));
            // Teardown
        }

        [Fact]
        public void WriteInvokesDecoratedWriterWhenPrincipalIsAuthenticated()
        {
            // Fixture setup
            var principalStub = new Mock<IPrincipal>();
            principalStub.SetupGet(p => p.Identity.IsAuthenticated).Returns(true);
            Thread.CurrentPrincipal = principalStub.Object;

            var writerMock = new Mock<IMessageWriter>();
            var sut = new SecureMessageWriter(writerMock.Object);
            // Exercise system
            var message = "Whatever";
            sut.Write(message);
            // Verify outcome
            writerMock.Verify(w => w.Write(message));
            // Teardown
        }

        [Fact]
        public void WriteDoesNotInvokeWriterWhenPrincipalIsNotAuthenticated()
        {
            // Fixture setup
            var principalStub = new Mock<IPrincipal>();
            principalStub.SetupGet(p => p.Identity.IsAuthenticated).Returns(false);
            Thread.CurrentPrincipal = principalStub.Object;

            var writerMock = new Mock<IMessageWriter>();
            var sut = new SecureMessageWriter(writerMock.Object);
            // Exercise system
            sut.Write("Anonymous value");
            // Verify outcome
            writerMock.Verify(w => w.Write(It.IsAny<string>()), Times.Never());
            // Teardown
        }

        #region IDisposable Members

        public void Dispose()
        {
            Thread.CurrentPrincipal = this.originalPrincipal;
        }

        #endregion
    }
}
