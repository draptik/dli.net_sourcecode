using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Ploeh.Samples.HelloDI.CommandLine;
using Xunit;

namespace Ploeh.Samples.HelloDI.UnitTest
{
    public class SalutationTest
    {
        [Fact]
        public void CreateWithNullWriterWillThrow()
        {
            // Fixture setup
            // Exercise system and verify outcome
            Assert.Throws<ArgumentNullException>(() =>
                new Salutation(null));
            // Teardown
        }

        [Fact]
        public void ExclaimWillWriteCorrectMessageToMessageWriter()
        {
            // Fixture setup
            var writerMock = new Mock<IMessageWriter>();
            var sut = new Salutation(writerMock.Object);
            // Exercise system
            sut.Exclaim();
            // Verify outcome
            writerMock.Verify(w => w.Write("Hello DI!"));
            // Teardown
        }
    }
}
