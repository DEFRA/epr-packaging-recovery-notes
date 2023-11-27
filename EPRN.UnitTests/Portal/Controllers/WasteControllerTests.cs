using Microsoft.AspNetCore.Mvc;
using Moq;
using Portal.Controllers;
using Portal.Services.Interfaces;
using Portal.ViewModels;

namespace EPRN.UnitTests.Portal.Controllers
{
    [TestClass]
    public class WasteControllerTests
    {
        private WasteController _wasteController;
        private Mock<IWasteService> _mockWasteService;

        [TestInitialize]
        public void Init()
        {
            _mockWasteService = new Mock<IWasteService>();
            _wasteController = new WasteController(_mockWasteService.Object);
        }

        [TestMethod]
        public async Task Types_Return_Correctly()
        {
            // Arrange
            _mockWasteService.Setup(s => s.GetWasteTypesViewModel(It.IsAny<int>())).ReturnsAsync(new WasteTypesViewModel());

            // Act
            var result = await _wasteController.Types(2);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult.ViewData.Model);

            // check model is expected type
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(WasteTypesViewModel));

            // check view name
            Assert.IsNull(viewResult.ViewName); // It's going to return the view name of the action by default
        }

        [TestMethod]
        public async Task DuringWhichMonth_Return_Correctly()
        {
            // Arrange
            _mockWasteService.Setup(s => s.GetCurrentQuarter(It.IsAny<int>())).ReturnsAsync(new DuringWhichMonthRequestViewModel());

            // Act
            var result = await _wasteController.DuringWhichMonth(2);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult.ViewData.Model);

            // check model is expected type
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(DuringWhichMonthRequestViewModel));

            // check view name
            Assert.IsNull(viewResult.ViewName); // It's going to return the view name of the action by default
        }

        [TestMethod]
        public async Task Types_ThrowsNotFoundException_WhenNoIdSupplied()
        {
            // Arrange

            // Act
            var result = await _wasteController.Types((int?)null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task DuringWhichMonth_ThrowsNotFoundException_WhenNoIdSupplied()
        {
            // Act
            var result = await _wasteController.DuringWhichMonth((int?)null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Types_Saves_WithValidData()
        {
            // Arrange
            var wasteTypesViewModel = new WasteTypesViewModel
            {
                JourneyId = 1,
                SelectedWasteTypeId = 99
            };

            // Act
            var result = await _wasteController.Types(wasteTypesViewModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));

            var redirectToActionResult = result as RedirectToActionResult;
            Assert.AreEqual("Home", redirectToActionResult.ControllerName); // this will need to change eventually when we know where this redirects to
            Assert.AreEqual("Index", redirectToActionResult.ActionName); // this will need to change eventually when we know where this redirects to
        }

        [TestMethod]
        public async Task DuringWhichMonth_Saves_WithValidData()
        {
            // Arrange
            var duringWhichMonthRequestViewModel = new DuringWhichMonthRequestViewModel
            {
                JourneyId = 1,
                SelectedMonth = 11
            };

            // Act
            var result = await _wasteController.DuringWhichMonth(duringWhichMonthRequestViewModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));

            var redirectToActionResult = result as RedirectToActionResult;
            Assert.AreEqual("Home", redirectToActionResult.ControllerName); // this will need to change eventually when we know where this redirects to
            Assert.AreEqual("Index", redirectToActionResult.ActionName); // this will need to change eventually when we know where this redirects to
        }

        [TestMethod]
        public async Task Types_ReturnsCurrentView_WhenModelIsInvalid()
        {
            // Arrange
            var wasteTypesViewModel = new WasteTypesViewModel();
            _mockWasteService.Setup(s => s.GetWasteTypesViewModel(It.IsAny<int>())).ReturnsAsync(new WasteTypesViewModel());
            _wasteController.ModelState.AddModelError("Error", "Error");

            // Act
            var result = await _wasteController.Types(wasteTypesViewModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult.ViewData.Model);

            // check model is expected type
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(WasteTypesViewModel));

            // check view name
            Assert.IsNull(viewResult.ViewName); // It's going to return the view name of the action by default
        }

        [TestMethod]
        public async Task DuringWhichMonth_ReturnsCurrentView_WhenModelIsInvalid()
        {
            // Arrange
            var wasteTypesViewModel = new WasteTypesViewModel();
            _mockWasteService.Setup(s => s.GetWasteTypesViewModel(It.IsAny<int>())).ReturnsAsync(new WasteTypesViewModel());
            _wasteController.ModelState.AddModelError("Error", "Error");
            // Arrange
            var duringWhichMonthRequestViewModel = new DuringWhichMonthRequestViewModel();
            _mockWasteService.Setup(s => s.GetCurrentQuarter(It.IsAny<int>())).ReturnsAsync(new DuringWhichMonthRequestViewModel());
            _wasteController.ModelState.AddModelError("Error", "Error");

            // Act
            var result = await _wasteController.DuringWhichMonth(duringWhichMonthRequestViewModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult.ViewData.Model);

            // check model is expected type
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(DuringWhichMonthRequestViewModel));

            // check view name
            Assert.IsNull(viewResult.ViewName); // It's going to return the view name of the action by default
        }
    }
}
