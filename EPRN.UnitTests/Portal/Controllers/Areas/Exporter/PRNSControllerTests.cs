using EPRN.Common.Enums;
using EPRN.Portal.Areas.Exporter.Controllers;
using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels.PRNS;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EPRN.UnitTests.Portal.Controllers.Areas.Exporter
{
    [TestClass]
    public class PRNSControllerTests
    {
        private PRNSController _prnController;
        private Mock<IPRNService> _mockPrnService;
        private Func<Category, IPRNService> _mockPrnServiceFactory;

        [TestInitialize]
        public void Init()
        {
            _mockPrnService = new Mock<IPRNService>();
            _mockPrnServiceFactory = new Func<Category, IPRNService>((category) => _mockPrnService.Object);
            _prnController = new PRNSController(_mockPrnServiceFactory);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ThrowsException_NullParameterProvided()
        {
            _prnController = new PRNSController(null);
        }

        [TestMethod]
        public async Task Tonnes_ReturnsNotFound_NoIdProvided()
        {
            // arrange
            int? id = null;

            // act
            var result = await _prnController.Tonnes(id);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Tonnes_CallsService_ValidParameter()
        {
            // arrange
            var id = 3;

            // act
            var result = await _prnController.Tonnes(id);

            // assert
            _mockPrnService.Verify(s => 
                s.GetTonnesViewModel(
                    It.Is<int>(p => p == id)), 
                Times.Once);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            var viewResult = (ViewResult)result;

            // the view is the same as the action, therefore no name expected
            Assert.IsNull(viewResult.ViewName);
        }

        [TestMethod]
        public async Task Tonnes_ReturnsView_WhenModelStateInvalid()
        {
            // arrange
            var tonnesViewModel = new TonnesViewModel();

            _prnController.ModelState.AddModelError("error", "error message"); // ensure model state is invalid

            // act
            var result = await _prnController.Tonnes(tonnesViewModel);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            var viewResult = result as ViewResult;
            Assert.IsNull(viewResult.ViewName);
        }

        [TestMethod]
        public async Task Tonnes_Redirects_WhenModelStateIsValid()
        {
            // arrange
            var id = 6;
            var tonnes = 5.7;
            var tonnesViewModel = new TonnesViewModel
            {
                JourneyId = id,
                Tonnes = tonnes,
            };

            // act
            var result = await _prnController.Tonnes(tonnesViewModel);

            // assert
            _mockPrnService.Verify(s =>
                s.SaveTonnes(
                    It.Is<TonnesViewModel>(p => p.JourneyId == id && p.Tonnes == tonnes)),
                Times.Once());
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));

            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Create", redirectResult.ActionName);
            Assert.AreEqual("Prns", redirectResult.ControllerName);
            var routeValues = redirectResult.RouteValues.FirstOrDefault();

            Assert.IsNotNull(routeValues);
            Assert.AreEqual("area", routeValues.Key);
            Assert.AreEqual(string.Empty, routeValues.Value);
        }

        [TestMethod]
        public async Task Confirmation_ReturnsNotFound_WhenNoParametersSupplied()
        {
            // arrange
            int? id = null;

            // act
            var result = await _prnController.Confirmation(id);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Confirmation_ReturnsViewResult_WhenValidParametersSupplied()
        {
            // arrange
            var id = 3;

            // act
            var result = await _prnController.Confirmation(id);

            // assert
            _mockPrnService.Verify(s => 
                s.GetConfirmation(
                    It.Is<int>(p => p == id)), 
                Times.Once);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            var viewResult = (ViewResult)result;

            // the view is the same as the action, therefore no name expected
            Assert.IsNull(viewResult.ViewName);
        }
    }
}
