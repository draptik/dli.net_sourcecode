using System;
using System.IO;
using Ploeh.AutoFixture;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.Samples.Commerce.UpdateCurrency.ApplicationServices.UnitTest
{
    public class HelpCommandTest : IDisposable
    {
        [Theory, AutoMoqData]
        public void SutIsCommand(HelpCommand sut)
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.IsAssignableFrom<ICommand>(sut);
            // Teardown
        }

        [Theory, AutoMoqData]
        public void ExecuteWillWriteCorrectOutput(HelpCommand sut)
        {
            // Fixture setup
            var expectedOutput = "Usage: UpdateCurrency <DKK | EUR | USD> <DKK | EUR | USD> <rate>." + Environment.NewLine;

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                // Exercise system
                sut.Execute();
                // Verify outcome
                Assert.Equal(expectedOutput, sw.ToString());
                // Teardown
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            var standardOut = new StreamWriter(Console.OpenStandardOutput());
            standardOut.AutoFlush = true;
            Console.SetOut(standardOut);
        }

        #endregion
    }
}
