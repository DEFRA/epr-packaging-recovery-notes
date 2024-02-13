using EPRN.Common.Dtos;
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
        private WasteCommonViewModel _wasteCommonViewModel;

        [TestInitialize]
        public void Init()
        {
            _wasteCommonViewModel = new WasteCommonViewModel { CompanyName = "abc", CompanyReferenceId = Guid.NewGuid().ToString(), WasteName = "some waste" };
            _mockWasteService = new Mock<IWasteService>();
            _mockWasteService.Setup(x => x.GetAccredidationLimit(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<double>())).ReturnsAsync(new AccredidationLimitViewModel());
            _homeServiceFactory = new Mock<IHomeServiceFactory>();
            var exporterHomeService = new Mock<IUserBasedService>();
            exporterHomeService.Setup(service => service.GetCheckAnswers(It.IsAny<int>())).ReturnsAsync(new CYAViewModel() { UserRole = UserRole.Reprocessor});
            _homeServiceFactory.Setup(x => x.CreateHomeService()).Returns(exporterHomeService.Object);
            _wasteController = new WasteController(_mockWasteService.Object, _homeServiceFactory.Object, _wasteCommonViewModel);
        }

        [TestMethod]
        public async Task RecordWaste_WithNoIdParameter_CallsServiceCorrectly()
        {
            // arrange

            // act
            var result = await _wasteController.RecordWaste((int?)null);

            // assert
            _mockWasteService.Verify(s => s.GetWasteTypesViewModel(It.Is<int?>(p => p == null)), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsNull(((ViewResult)result).ViewName);

        }

        [TestMethod]
        public async Task RecordWaste_WithIdParameter_CallsServiceCorrectly()
        {
            // arrange
            var id = 45;

            // act
            var result = await _wasteController.RecordWaste(id);

            // assert
            _mockWasteService.Verify(s => 
                s.GetWasteTypesViewModel(
                    It.Is<int?>(p => p.HasValue && p.Value == id)), 
                Times.Once);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsNull(((ViewResult)result).ViewName);
        }

        [TestMethod]
        public async Task RecordWaste_WithViewModel_CallsServiceCorrectly()
        {
            // arrange
            var wasteTypesViewModel = new WasteTypeViewModel
            {
                Id = 7345,
                MaterialId = 2435
            };

            // act
            var result = await _wasteController.RecordWaste(wasteTypesViewModel);

            // assert
            _mockWasteService.Verify(s =>
                s.SaveSelectedWasteType(
                    It.Is<WasteTypeViewModel>(p => p == wasteTypesViewModel)), 
                Times.Once);
        }

        [TestMethod]
        public async Task RecordWaste_WithInvalidViewModel_CallsServiceCorrectly()
        {
            // arrange
            _wasteController.ModelState.AddModelError("Error", "Error");

            // act
            var result = await _wasteController.RecordWaste((WasteTypeViewModel)null);

            // assert
            _mockWasteService.Verify(s =>
                s.SaveSelectedWasteType(
                    It.IsAny<WasteTypeViewModel>()),
                Times.Never);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task DuringWhichMonth_Return_Correctly()
        {
            // Arrange
            int Id = 8;
            int currentMonth = DateTime.Now.Month;

            _mockWasteService.Setup(s => s.GetQuarterForCurrentMonth(
                It.Is<int>(id => id == Id))).ReturnsAsync(new DuringWhichMonthRequestViewModel());

            // Act
            var result = await _wasteController.DuringWhichMonth(Id);

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
        public async Task DuringWhichMonth_ThrowsNotFoundException_WhenNoIdSupplied()
        {
            // Act
            var result = await _wasteController.DuringWhichMonth((int?)null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task DuringWhichMonth_Saves_WithValidData()
        {
            // Arrange
            var duringWhichMonthRequestViewModel = new DuringWhichMonthRequestViewModel
            {
                Id = 1,
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
                Id = 1,
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
                Id = 1,
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
        public async Task Tonnes_WithValidId_ReturnsViewWithModel()
        {
            // Arrange
            var Id = 3;
            var exportTonnageViewModel = new ExportTonnageViewModel();

            _mockWasteService.Setup(s => s.GetExportTonnageViewModel(3)).ReturnsAsync(exportTonnageViewModel);

            // Act
            var result = await _wasteController.Tonnes(Id);

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
                Id = 6,
                ExportTonnes = 44.5
            };

            // Act
            var result = await _wasteController.Tonnes(exportTonnageViewModel, false);

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
                Id = 6,
                ExportTonnes = 44.5
            };

            // Act
            var result = await _wasteController.Tonnes(exportTonnageViewModel, false);

            // Assert
            _mockWasteService.Verify(s => s.SaveTonnage(It.IsAny<ExportTonnageViewModel>()), Times.Never);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            var viewResult = result as ViewResult;
            Assert.AreEqual(exportTonnageViewModel, viewResult.ViewData.Model);
            Assert.IsNull(viewResult.ViewName); // view is being returned as the same as the action
        }

        [TestMethod]
        public async Task SaveTonnes_WithValidData_AndExcessTonnage_RedirectsToAlertPage()
        {

            // Arrange class level objects
            var vm = new AccredidationLimitViewModel { ExcessOfLimit = -100 };
            _mockWasteService.Setup(x => x.GetAccredidationLimit(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<double>())).ReturnsAsync(vm);

            // Arrange
            var exportTonnageViewModel = new ExportTonnageViewModel
            {
                Id = 6,
                ExportTonnes = 44.5
            };

            // Act
            var result = await _wasteController.Tonnes(exportTonnageViewModel, true);

            // Assert
            _mockWasteService.Verify(s => s.SaveTonnage(It.Is<ExportTonnageViewModel>(p => p == exportTonnageViewModel)), Times.Never);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public async Task BaledWithWire_ReturnCurrentView_WhenModelIsInvalid()
        {
            var baledWithWireModel = new BaledWithWireViewModel();
            _mockWasteService.Setup(s => s.GetBaledWithWireModel(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new BaledWithWireViewModel());


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
            Assert.IsNull(viewResult.ViewName);
        }

        [TestMethod]
        public async Task BaledWithWire_Return_Correctly()
        {
            // Arrange
            var baledWithWireModel = new BaledWithWireViewModel
            {
                Id = 1,
                BaledWithWireDeductionPercentage = 100,
                BaledWithWire = true
            };

            _mockWasteService.Setup(s => s.GetBaledWithWireModel(It.IsAny<int>())).ReturnsAsync(baledWithWireModel);

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
            Assert.IsNull(viewResult.ViewName);
        }

        [TestMethod]
        public async Task BaledWithWire_Saves_WithValidData()
        {
            // Arrange
            var Id = 3;
            var baledWithWireModel = new BaledWithWireViewModel
            {
                Id = Id,
                BaledWithWire = true
            };

            // Act
            var result = await _wasteController.BaledWithWire(baledWithWireModel);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));

            var redirectToActionResult = result as RedirectToActionResult;
            Assert.AreEqual("Note", redirectToActionResult.ActionName);
            Assert.AreEqual(Id, redirectToActionResult.RouteValues.FirstOrDefault(rv => rv.Key == "id").Value);
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
            var Id = 38;

            NoteViewModel noteViewModel = new NoteViewModel
            {
                Id = Id
            };

            _mockWasteService.Setup(s => s.GetNoteViewModel(It.Is<int>(p => p == Id))).ReturnsAsync(noteViewModel);

            // Act
            var result = await _wasteController.Note(Id);

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
            _mockWasteService.Verify(s => s.GetNoteViewModel(It.Is<int>(p => p == Id)), Times.Once);
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
                Id = 4
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

        //[TestMethod]
        //public async Task Note_ReturnsCorrectView_WhenModelIsInvalid()
        //{
        //    // Arrange
        //    var wasteTypesViewModel = new WasteTypesViewModel();
        //    _mockWasteService.Setup(s => s.GetWasteTypesViewModel(It.IsAny<int>())).ReturnsAsync(new WasteTypesViewModel());
        //    _wasteController.ModelState.AddModelError("Error", "Error");

        //    // Arrange
        //    var noteViewModel = new NoteViewModel();

        //    _mockWasteService.Setup(s => s.GetNoteViewModel(It.IsAny<int>())).ReturnsAsync(noteViewModel);
        //    _wasteController.ModelState.AddModelError("Error", "Error");

        //    // Act
        //    var result = await _wasteController.Note(noteViewModel);

        //    // Assert
        //    Assert.IsNotNull(result);
        //    Assert.IsInstanceOfType(result, typeof(ViewResult));

        //    var viewResult = result as ViewResult;
        //    Assert.IsNotNull(viewResult.ViewData.Model);

        //    // check model is expected type
        //    Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(NoteViewModel));

            // check view name
            //Assert.IsNull(viewResult.ViewName); // It's going to return the view name of the action by default
        //}

        [TestMethod]
        public async Task CheckYourAnswers_ReturnsCorrectViewModel_ForValidJourneyId()
        {
            // Arrange
            int journeyId = 1;

            // Act
            var result = await _wasteController.CheckYourAnswers(journeyId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult.ViewData.Model);

            // check model is expected type
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(CYAViewModel));

            // check view name
            Assert.IsNotNull(viewResult.ViewName); // It's going to return the view name of the action by default
        }

        [TestMethod]
        public async Task CheckYourAnswers_ReturnsNotFound_ForNullJourneyId()
        {
            // Arrange

            // Act
            var result = await _wasteController.CheckYourAnswers((int?)null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task CheckYourAnswers_ReturnsCorrectView_WhenModelIsInvalid()
        {
            // Arrange
            _wasteController.ModelState.AddModelError("Error", "Error");

            // Act
            var result = await _wasteController.CheckYourAnswers(new CYAViewModel());

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult.ViewData.Model);

            // check model is expected type
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(CYAViewModel));

            // check view name
            Assert.IsNull(viewResult.ViewName); // It's going to return the view name of the action by default
        }

        [TestMethod]
        public async Task CheckYourAnswers_ReturnsRedirectToAction_WhenModelIsValid()
        {
            // Arrange

            // Act
            var result = await _wasteController.CheckYourAnswers(new CYAViewModel());

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));

            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectResult.ActionName);
            Assert.AreEqual("Home", redirectResult.ControllerName);
        }

    }
}
