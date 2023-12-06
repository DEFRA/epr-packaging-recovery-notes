using EPRN.Common.Enums;
using EPRN.Portal.Controllers;
using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;

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
            int journeyId = 8;
            int currentMonth = DateTime.Now.Month;

            _mockWasteService.Setup(s => s.GetQuarterForCurrentMonth(
                It.Is<int>(id => id == journeyId),
                It.Is<int>(month => month == currentMonth))).ReturnsAsync(new DuringWhichMonthRequestViewModel());

            // Act
            var result = await _wasteController.DuringWhichMonth(journeyId);

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
        public async Task DuringWhichMonth_ReturnsCorrectView_WhenModelIsInvalid()
        {
            // Arrange
            var wasteTypesViewModel = new WasteTypesViewModel();
            _mockWasteService.Setup(s => s.GetWasteTypesViewModel(It.IsAny<int>())).ReturnsAsync(new WasteTypesViewModel());
            _wasteController.ModelState.AddModelError("Error", "Error");

            // Arrange
            var whatHaveYouDoneWasteModel = new WhatHaveYouDoneWasteModel();
            _mockWasteService.Setup(s => s.GetWasteModel(It.IsAny<int>())).ReturnsAsync(new WhatHaveYouDoneWasteModel());
            _wasteController.ModelState.AddModelError("Error", "Error");

            // Arrange
            var duringWhichMonthRequestViewModel = new DuringWhichMonthRequestViewModel();

            _mockWasteService.Setup(s => s.GetQuarterForCurrentMonth(
                It.IsAny<int>(),
                It.IsAny<int>()))
                .ReturnsAsync(new DuringWhichMonthRequestViewModel());

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

        [TestMethod]
        public async Task WhatHaveYouDoneWaste_ReturnCurrentView_WhenModelIsInvalid()
        {
            var whatHaveYouDoneWithWasteViewModel = new WhatHaveYouDoneWasteModel();
            _mockWasteService.Setup(s => s.GetWasteModel(It.IsAny<int>())).ReturnsAsync(new WhatHaveYouDoneWasteModel());

            // Act
            var result = await _wasteController.WhatHaveYouDoneWaste(It.IsAny<int>());

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult.ViewData.Model);

            // check model is expected type
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(WhatHaveYouDoneWasteModel));

            // check view name
            Assert.IsNull(viewResult.ViewName);
        }

        [TestMethod]
        public async Task WhatHaveYouDoneWaste_Return_Correctly()
        {
            // Arrange
            var whatHaveYouDoneWasteModel = new WhatHaveYouDoneWasteModel
            {
                JourneyId = 1,
                WhatHaveYouDone = DoneWaste.ReprocessedIt
            };

            _mockWasteService.Setup(s => s.GetWasteModel(1)).ReturnsAsync(whatHaveYouDoneWasteModel);

            // Act
            var result = await _wasteController.WhatHaveYouDoneWaste(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult.ViewData.Model);

            // check model is expected type
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(WhatHaveYouDoneWasteModel));

            // check view name
            Assert.IsNull(viewResult.ViewName); // It's going to return the view name of the action by default
        }

        [TestMethod]
        public async Task WhatHaveYouDoneWaste_Saves_WithValidData()
        {
            // Arrange
            var whatHaveYouDoneWasteModel = new WhatHaveYouDoneWasteModel
            {
                JourneyId = 1,
                WhatHaveYouDone = DoneWaste.SentItOn
            };

            // Act
            var result = await _wasteController.WhatHaveYouDoneWaste(whatHaveYouDoneWasteModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));

            var redirectToActionResult = result as RedirectToActionResult;
            Assert.AreEqual("Home", redirectToActionResult.ControllerName); // this will need to change eventually when we know where this redirects to
            Assert.AreEqual("Index", redirectToActionResult.ActionName); // this will need to change eventually when we know where this redirects to
        }

        [TestMethod]
        public async Task WhatHaveYouDoneWaste_ThrowsNotFoundException_WhenNoIdSupplied()
        {
            // Act
            var result = await _wasteController.WhatHaveYouDoneWaste((int?)null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetWasteRecordStatus_Return_RecordCompleteView_ValidId()
        {
            // Arrange
            _mockWasteService.Setup(s => s.GetWasteRecordStatus(It.IsAny<int>())).ReturnsAsync(new WasteRecordStatusViewModel { WasteRecordStatus = Common.Enums.WasteRecordStatuses.Complete });

            // Act
            var result = await _wasteController.GetWasteRecordStatus(2);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult.ViewData.Model);

            // check model is expected type
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(WasteRecordStatusViewModel));

            // check view name
            Assert.IsNotNull(viewResult.ViewName);
            Assert.AreEqual("WasteRecordCompleteStatus", viewResult.ViewName);

        }

        [TestMethod]
        public async Task GetWasteRecordStatus_Return_RecordNotCompleteView_InValidId()
        {
            // Arrange
            _mockWasteService.Setup(s => s.GetWasteRecordStatus(It.IsAny<int>())).ReturnsAsync(new WasteRecordStatusViewModel { WasteRecordStatus = Common.Enums.WasteRecordStatuses.Incomplete });

            // Act
            var result = await _wasteController.GetWasteRecordStatus(2);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult.ViewData.Model);

            // check model is expected type
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(WasteRecordStatusViewModel));

            // check view name
            Assert.IsNotNull(viewResult.ViewName);
            Assert.AreEqual("WasteRecordStatus", viewResult.ViewName);
        }

        [TestMethod]
        public void Tonnes_WithValidId_ReturnsViewWithModel()
        {
            // Arrange
            var journeyId = 3;
            var exportTonnageViewModel = new ExportTonnageViewModel();

            _mockWasteService.Setup(s => s.GetExportTonnageViewModel(3)).Returns(exportTonnageViewModel);

            // Act
            var result = _wasteController.Tonnes(journeyId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            var viewResult = result as ViewResult;
            Assert.AreEqual(exportTonnageViewModel, viewResult.ViewData.Model);
        }

        [TestMethod]
        public void Tonnes_WithNoId_ReturnsNotFound()
        {
            // Arrange

            // Act
            var result = _wasteController.Tonnes((int?)null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

            _mockWasteService.Verify(s => s.GetExportTonnageViewModel(It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public async Task SaveTonnes_WithValidData_Saves_Succesfully()
        {
            // Arrange
            var exportTonnageViewModel = new ExportTonnageViewModel
            {
                JourneyId = 6,
                ExportTonnes = 44.5
            };

            // Act
            var result = await _wasteController.Tonnes(exportTonnageViewModel);

            // Assert
            _mockWasteService.Verify(s => s.SaveTonnage(It.Is<ExportTonnageViewModel>(p => p == exportTonnageViewModel)), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }

        [TestMethod]
        public async Task SaveTonnes_WithInvalidData_ReturnsView()
        {
            // Arrange
            _wasteController.ModelState.AddModelError("Error", "Error");
            var exportTonnageViewModel = new ExportTonnageViewModel
            {
                JourneyId = 6,
                ExportTonnes = 44.5
            };

            // Act
            var result = await _wasteController.Tonnes(exportTonnageViewModel);

            // Assert
            _mockWasteService.Verify(s => s.SaveTonnage(It.IsAny<ExportTonnageViewModel>()), Times.Never);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            var viewResult = result as ViewResult;
            Assert.AreEqual(exportTonnageViewModel, viewResult.ViewData.Model);
            Assert.IsNull(null, viewResult.ViewName); // view is being returned as the same as the action
        }
    }
}
