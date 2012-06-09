using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Extensions;
using Xunit;
using Ploeh.AutoFixture.Xunit;
using Moq;
using Ploeh.Samples.AssortedCodeSnippets.AbstractFactory.Route;

namespace Ploeh.Samples.AssortedCodeSnippetsUnitTest.AbstractFactory.Route
{
    public class RouteControllerTest
    {
        [Theory, AutoMoqData]
        public void GetRouteReturnsCorrectRoute([Frozen]Mock<IRouteAlgorithmFactory> factoryStub, Mock<IRouteAlgorithm> routeAlgorithmStub, RouteSpecification specification, RouteType selectedAlgorithm, IRoute expectedResult, RouteController sut)
        {
            // Fixture setup
            factoryStub.Setup(f => f.CreateAlgorithm(selectedAlgorithm)).Returns(routeAlgorithmStub.Object);
            routeAlgorithmStub.Setup(a => a.CalculateRoute(specification)).Returns(expectedResult);
            // Exercise system
            var result = sut.GetRoute(specification, selectedAlgorithm);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }
    }
}
