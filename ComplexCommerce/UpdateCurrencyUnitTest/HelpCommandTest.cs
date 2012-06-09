using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Ploeh.Samples.Commerce.UpdateCurrency.CommandLine.UnitTest
{
    [TestClass]
    public class HelpCommandTest
    {
        public HelpCommandTest()
        {
        }

        [TestCleanup]
        public void TeardownFixture()
        {
            var standardOut = new StreamWriter(Console.OpenStandardOutput());
            standardOut.AutoFlush = true;
            Console.SetOut(standardOut);
        }

        [TestMethod]
        public void SutIsCommand()
        {
            // Fixture setup
            var fixture = new CurrencyFixture();
            // Exercise system
            var sut = fixture.CreateAnonymous<HelpCommand>();
            // Verify outcome
            Assert.IsInstanceOfType(sut, typeof(ICommand));
            // Teardown
        }

        [TestMethod]
        public void ExecuteWillWriteCorrectOutput()
        {
            // Fixture setup
            var fixture = new CurrencyFixture();
            var expectedOutput = "Usage: UpdateCurrency <DKK | EUR | USD> <DKK | EUR | USD> <rate>." + Environment.NewLine;

            var sut = fixture.CreateAnonymous<HelpCommand>();

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                // Exercise system
                sut.Execute();
                // Verify outcome
                Assert.AreEqual(expectedOutput, sw.ToString(), "Execute");
                // Teardown
            }
        }
    }
}
