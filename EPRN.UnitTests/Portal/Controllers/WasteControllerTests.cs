using EPRN.Common.Enums;
using EPRN.Portal.Controllers;
using EPRN.Portal.Helpers.Interfaces;
using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels.Waste;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EPRN.UnitTests.Portal.Controllers
{
    [TestClass]
    public class WasteControllerTests
    {
        private WasteController _wasteController;
        private Mock<IWasteService> _mockWasteService;
        private Mock<IHomeServiceFactory> _homeServiceFactory;

        [TestInitialize]
        public void Init()
        {
            _mockWasteService = new Mock<IWasteService>();
            _homeServiceFactory = new Mock<IHomeServiceFactory>();
            var exporterHomeService = new Mock<IHomeService>();
            _homeServiceFactory.Setup(x => x.CreateHomeService()).Returns(exporterHomeService.Object);
            _wasteController = new WasteController(_mockWasteService.Object, _homeServiceFactory.Object);
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
        public async Task Types_CreatesNewJourney_WhenNoIdSupplied()
        {
            // Arrange

            // Act
            var result = await _wasteController.Types((int?)null);

            // Assert
            Assert.IsNotNull(result);

            _mockWasteService.Verify(s => s.CreateJourney(), Times.Once);
            var redirectToActionResult = result as RedirectToActionResult;
            Assert.IsNull(redirectToActionResult.ControllerName); // this will need to change eventually when we know where this redirects to
            Assert.AreEqual("Types", redirectToActionResult.ActionName); // this will need to change eventually when we know where this redirects to
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
            Assert.IsNull(redirectToActionResult.ControllerName); // this will need to change eventually when we know where this redirects to
            Assert.AreEqual("Done", redirectToActionResult.ActionName); // this will need to change eventually when we know where this redirects to
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
            Assert.IsNull(redirectToActionResult.ControllerName); // this will need to change eventually when we know where this redirects to
            Assert.AreEqual("SubTypes", redirectToActionResult.ActionName); // this will need to change eventually when we know where this redirects to
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
            Assert.IsNull(redirectToActionResult.ControllerName); // this will need to change eventually when we know where this redirects to
            Assert.AreEqual("Month", redirectToActionResult.ActionName); // this will need to change eventually when we know where this redirects to
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
        public async Task Tonnes_WithValidId_ReturnsViewWithModel()
        {
            // Arrange
            var journeyId = 3;
            var exportTonnageViewModel = new ExportTonnageViewModel();

            _mockWasteService.Setup(s => s.GetExportTonnageViewModel(3)).ReturnsAsync(exportTonnageViewModel);

            // Act
            var result = await _wasteController.Tonnes(journeyId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            var viewResult = result as ViewResult;
            Assert.AreEqual(exportTonnageViewModel, viewResult.ViewData.Model);
        }

        [TestMethod]
        public async Task Tonnes_WithNoId_ReturnsNotFound()
        {
            // Arrange

            // Act
            var result = await _wasteController.Tonnes((int?)null);

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
            Assert.IsNull(viewResult.ViewName); // view is being returned as the same as the action
        }

        [TestMethod]
        public async Task BaledWithWire_ReturnCurrentView_WhenModelIsInvalid()
        {
            var baledWithWireModel = new BaledWithWireViewModel();
            _mockWasteService.Setup(s => s.GetBaledWithWireModel(It.IsAny<int>())).ReturnsAsync(new BaledWithWireViewModel());

            // Act
            var result = await _wasteController.BaledWithWire(0);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult.ViewData.Model);

            // check model is expected type
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(BaledWithWireViewModel));

            // check view name
            Assert.AreEqual("BaledWithWire", viewResult.ViewName);
        }

        [TestMethod]
        public async Task BaledWithWire_Return_Correctly()
        {
            // Arrange
            var baledWithWireModel = new BaledWithWireViewModel
            {
                JourneyId = 1,
                BaledWithWire = true
            };

            _mockWasteService.Setup(s => s.GetBaledWithWireModel(1)).ReturnsAsync(baledWithWireModel);

            // Act
            var result = await _wasteController.BaledWithWire(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult.ViewData.Model);

            // check model is expected type
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(BaledWithWireViewModel));

            // check view name
            Assert.AreEqual("BaledWithWire", viewResult.ViewName);
        }

        [TestMethod]
        public async Task BaledWithWire_Saves_WithValidData()
        {
            // Arrange
            var journeyId = 3;
            var baledWithWireModel = new BaledWithWireViewModel
            {
                JourneyId = journeyId,
                BaledWithWire = true
            };

            // Act
            var result = await _wasteController.BaledWithWire(baledWithWireModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));

            var redirectToActionResult = result as RedirectToActionResult;
            Assert.AreEqual("Note", redirectToActionResult.ActionName);
            Assert.AreEqual(journeyId, redirectToActionResult.RouteValues.FirstOrDefault(rv => rv.Key == "id").Value);
        }

        [TestMethod]
        public async Task BaledWithWire_ThrowsNotFoundException_WhenNoIdSupplied()
        {
            // Act
            var result = await _wasteController.BaledWithWire((int?)null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Note_Return_Correctly()
        {
            // Arrange
            var journeyId = 38;
            var wasteType = "testWasteType";

            NoteViewModel noteViewModel = new NoteViewModel
            {
                JourneyId = journeyId,
                WasteType = wasteType
            };

            _mockWasteService.Setup(s => s.GetNoteViewModel(It.Is<int>(p => p == journeyId))).ReturnsAsync(noteViewModel);

            // Act
            var result = await _wasteController.Note(journeyId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult.ViewData.Model);

            // check model is expected type
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(NoteViewModel));

            // check view name
            Assert.IsNull(viewResult.ViewName); // It's going to return the view name of the action by default

            // Ensure that the service is called only once
            _mockWasteService.Verify(s => s.GetNoteViewModel(It.Is<int>(p => p == journeyId)), Times.Once);
        }

        [TestMethod]
        public async Task Note_ThrowsNotFoundException_WhenNoIdSupplied()
        {
            // Arrange

            // Act
            var result = await _wasteController.Note((int?)null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Note_Saves_WithValidData()
        {
            // Arrange

            NoteViewModel noteViewModel = new NoteViewModel
            {
                JourneyId = 4,
                WasteType = "testWasteType"
            };

            // Act
            var result = await _wasteController.Note(noteViewModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));

            var redirectToActionResult = result as RedirectToActionResult;
            Assert.AreEqual("Home", redirectToActionResult.ControllerName); // this will need to change eventually when we know where this redirects to
            Assert.AreEqual("Index", redirectToActionResult.ActionName); // this will need to change eventually when we know where this redirects to
            _mockWasteService.Verify(s => s.SaveNote(It.Is<NoteViewModel>(p => p == noteViewModel)), Times.Once);
        }

        [TestMethod]
        public async Task Note_ReturnsCorrectView_WhenModelIsInvalid()
        {
            // Arrange
            var wasteTypesViewModel = new WasteTypesViewModel();
            _mockWasteService.Setup(s => s.GetWasteTypesViewModel(It.IsAny<int>())).ReturnsAsync(new WasteTypesViewModel());
            _wasteController.ModelState.AddModelError("Error", "Error");

            // Arrange
            var noteViewModel = new NoteViewModel();

            _mockWasteService.Setup(s => s.GetNoteViewModel(It.IsAny<int>())).ReturnsAsync(noteViewModel);
            _wasteController.ModelState.AddModelError("Error", "Error");

            // Act
            var result = await _wasteController.Note(noteViewModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult.ViewData.Model);

            // check model is expected type
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(NoteViewModel));

            // check view name
            Assert.IsNull(viewResult.ViewName); // It's going to return the view name of the action by default
        }
    }
}
