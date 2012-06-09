using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Ploeh.Samples.AssortedCodeSnippets.PropertyInjection;
using Moq;
using Xunit;

namespace Ploeh.Samples.AssortedCodeSnippetsUnitTest.PropertyInjection
{
    public class SomeClassTest
    {
        [Fact]
        public void DoSomethingWithDependencyWillThrow()
        {
            var mc = new SomeClass();
            Assert.Throws<NullReferenceException>(() =>
                mc.DoSomething("Ploeh"));
        }

        [Fact]
        public void DoSomethingWillInvokeDependency()
        {
            // Fixture setup
            string message = "ploeh";
            string expectedResult = "fnaah";

            var sut = new SomeClass();

            var stub = new Mock<ISomeInterface>();
            stub.Setup(s => s.DoStuff(message)).Returns(expectedResult);
            sut.Dependency = stub.Object;
            // Exercise system
            var result = sut.DoSomething(message);
            // Verify outcome
            Assert.Equal<string>(expectedResult, result);
            // Teardown
        }
    }
}
