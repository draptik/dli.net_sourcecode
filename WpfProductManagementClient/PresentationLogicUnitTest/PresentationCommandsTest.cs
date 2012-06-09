using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xunit;

namespace Ploeh.Samples.ProductManagement.PresentationLogic.Wpf.UnitTest
{
    public class PresentationCommandsTest
    {
        [Fact]
        public void AcceptIsRoutedCommand()
        {
            // Fixture setup
            // Exercise system
            var result = PresentationCommands.Accept;
            // Verify outcome
            Assert.IsAssignableFrom<RoutedCommand>(result);
            // Teardown
        }

        [Fact]
        public void AcceptNameIsCorrect()
        {
            // Fixture setup
            var sut = PresentationCommands.Accept;
            // Exercise system
            var result = sut.Name;
            // Verify outcome
            Assert.Equal("Accept", result);
            // Teardown
        }
    }
}
