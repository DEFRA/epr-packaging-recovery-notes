using EPRN.Portal.Helpers.Filters;
using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels.Waste;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace EPRN.UnitTests.Portal.Helpers
{
    [TestClass]
    public class WasteTypeActionFilterTests
    {
        private Mock<IWasteService> _mockWasteService;
        private WasteTypeActionFilter _wasteTypeActionFilter;

        [TestInitialize]
        public void Init()
        {
            _mockWasteService = new Mock<IWasteService>();
            _wasteTypeActionFilter = new WasteTypeActionFilter(_mockWasteService.Object);
        }

        [TestMethod]
        public void Constructor_ThrowsException_WhenNullObjectPassedIn()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(() => new WasteTypeActionFilter(null));
            
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(exception, typeof(ArgumentNullException));
            Assert.AreEqual("Value cannot be null. (Parameter 'wasteService')", exception.Message);
        }

        [TestMethod]
        public void OnActionExecuting_RetrievesWasteType_WhenIdExists()
        {
            // Arrange
            var id = 56;
            var wasteType = "Steel";
            var modelState = new ModelStateDictionary();
            var mockHttpContext = new Mock<HttpContext>();
            var serviceProvider = new Mock<IServiceProvider>();
            var wasteCommonViewModel = new WasteCommonViewModel();

            var actionContext = new ActionContext(
                mockHttpContext.Object,
                Mock.Of<RouteData>(),
                Mock.Of<ActionDescriptor>(),
                modelState
            );

            var mockHttpRequest = new Mock<HttpRequest>();
            var routes = new RouteValueDictionary();
            
            mockHttpContext.Setup(c => c.Request).Returns(mockHttpRequest.Object);
            mockHttpContext.Setup(c => c.RequestServices).Returns(serviceProvider.Object);
            mockHttpRequest.Setup(r => r.RouteValues).Returns(routes);
            routes["id"] = id.ToString();

            serviceProvider.Setup(sp => sp.GetService(typeof(WasteCommonViewModel))).Returns(wasteCommonViewModel);
            _mockWasteService.Setup(s => s.GetWasteType(id)).ReturnsAsync(wasteType);

            var actionExecutingContext = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                Mock.Of<Controller>()
            );

            // Act
            _wasteTypeActionFilter.OnActionExecuting(actionExecutingContext);

            // Assert
            _mockWasteService.Verify(s => s.GetWasteType(It.Is<int>(p => p == id)), Times.Once);
            Assert.AreEqual(wasteType, wasteCommonViewModel.WasteName);
        }

        [TestMethod]
        public void OnActionExecuting_DoesNotRetrievesWasteType_WhenIdDoesNotExists()
        {
            // Arrange
            var modelState = new ModelStateDictionary();
            var mockHttpContext = new Mock<HttpContext>();
            var serviceProvider = new Mock<IServiceProvider>();
            var wasteCommonViewModel = new WasteCommonViewModel();

            var actionContext = new ActionContext(
                mockHttpContext.Object,
                Mock.Of<RouteData>(),
                Mock.Of<ActionDescriptor>(),
                modelState
            );

            var mockHttpRequest = new Mock<HttpRequest>();
            var routes = new RouteValueDictionary();

            serviceProvider.Setup(sp => sp.GetService(typeof(WasteCommonViewModel))).Returns(wasteCommonViewModel);
            mockHttpContext.Setup(c => c.Request).Returns(mockHttpRequest.Object);
            mockHttpContext.Setup(c => c.RequestServices).Returns(serviceProvider.Object);
            mockHttpRequest.Setup(r => r.RouteValues).Returns(routes);
            
            var actionExecutingContext = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                Mock.Of<Controller>()
            );

            // Act
            _wasteTypeActionFilter.OnActionExecuting(actionExecutingContext);

            // Assert
            _mockWasteService.Verify(s => s.GetWasteType(It.IsAny<int>()), Times.Never);
            Assert.IsNull(wasteCommonViewModel.WasteName);
        }

        [TestMethod]
        public void OnActionExecuting_DoesNotRetrievesWasteType_WhenIdIsNotAnInteger()
        {
            // Arrange
            var id = 56;
            var wasteType = "Steel";
            var modelState = new ModelStateDictionary();
            var mockHttpContext = new Mock<HttpContext>();
            var serviceProvider = new Mock<IServiceProvider>();
            var wasteCommonViewModel = new WasteCommonViewModel();

            var actionContext = new ActionContext(
                mockHttpContext.Object,
                Mock.Of<RouteData>(),
                Mock.Of<ActionDescriptor>(),
                modelState
            );

            var mockHttpRequest = new Mock<HttpRequest>();
            var routes = new RouteValueDictionary();

            mockHttpContext.Setup(c => c.Request).Returns(mockHttpRequest.Object);
            mockHttpContext.Setup(c => c.RequestServices).Returns(serviceProvider.Object);
            mockHttpRequest.Setup(r => r.RouteValues).Returns(routes);
            routes["id"] = "Blergh";

            serviceProvider.Setup(sp => sp.GetService(typeof(WasteCommonViewModel))).Returns(wasteCommonViewModel);
            _mockWasteService.Setup(s => s.GetWasteType(id)).ReturnsAsync(wasteType);

            var actionExecutingContext = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                Mock.Of<Controller>()
            );

            // Act
            _wasteTypeActionFilter.OnActionExecuting(actionExecutingContext);

            // Assert
            _mockWasteService.Verify(s => s.GetWasteType(It.IsAny<int>()), Times.Never);
            Assert.IsNull(wasteCommonViewModel.WasteName);
        }

        /// <summary>
        /// This unit test is only here for code coverage reasons. The method does nothing
        /// </summary>
        [TestMethod]
        public void OnActionExecuted_DoesNothing()
        {
            // Arrange
            var id = 56;
            var wasteType = "Steel";
            var modelState = new ModelStateDictionary();
            var mockHttpContext = new Mock<HttpContext>();
            var serviceProvider = new Mock<IServiceProvider>();
            var wasteCommonViewModel = new WasteCommonViewModel();

            var actionContext = new ActionContext(
                mockHttpContext.Object,
                Mock.Of<RouteData>(),
                Mock.Of<ActionDescriptor>(),
                modelState
            );

            var mockHttpRequest = new Mock<HttpRequest>();
            var routes = new RouteValueDictionary();

            mockHttpContext.Setup(c => c.Request).Returns(mockHttpRequest.Object);
            mockHttpContext.Setup(c => c.RequestServices).Returns(serviceProvider.Object);
            mockHttpRequest.Setup(r => r.RouteValues).Returns(routes);

            serviceProvider.Setup(sp => sp.GetService(typeof(WasteCommonViewModel))).Returns(wasteCommonViewModel);
            _mockWasteService.Setup(s => s.GetWasteType(id)).ReturnsAsync(wasteType);

            var actionExecutedContext = new ActionExecutedContext(
                actionContext,
                new List<IFilterMetadata>(),
                Mock.Of<Controller>()
            );

            // Act
            _wasteTypeActionFilter.OnActionExecuted(actionExecutedContext);

            // Assert
            _mockWasteService.Verify(s => s.GetWasteType(It.IsAny<int>()), Times.Never);
            Assert.IsNull(wasteCommonViewModel.WasteName);
        }
    }
}
