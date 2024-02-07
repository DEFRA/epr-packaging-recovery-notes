using AutoMapper;
using EPRN.Common.Dtos;
using EPRN.Common.Enums;
using EPRN.Portal.Configuration;
using EPRN.Portal.Helpers.Interfaces;
using EPRN.Portal.Resources;
using EPRN.Portal.RESTServices.Interfaces;
using EPRN.Portal.Services;
using EPRN.Portal.ViewModels.Waste;
using Microsoft.Extensions.Options;
using Moq;

namespace EPRN.UnitTests.Portal.Services
{
    [TestClass]
    public class WasteServiceTests
    {
        private WasteService _wasteService = null;
        private Mock<IMapper> _mockMapper = null;
        private Mock<IHttpWasteService> _mockHttpWasteService = null;
        private Mock<IHttpJourneyService> _mockHttpJourneyService = null;
        private Mock<ILocalizationHelper<WhichQuarterResources>> _mockLocalizationHelper = null;

        private Mock<IOptions<AppConfigSettings>> _mockConfigSettings = null;
        [TestInitialize]
        public void Init()
        {
            _mockMapper = new Mock<IMapper>();
            _mockHttpWasteService = new Mock<IHttpWasteService>();
            _mockHttpJourneyService = new Mock<IHttpJourneyService>();
            _mockLocalizationHelper = new Mock<ILocalizationHelper<WhichQuarterResources>>();
            _mockConfigSettings = new Mock<IOptions<AppConfigSettings>>();
            _wasteService = new WasteService(
                _mockMapper.Object,
                _mockHttpWasteService.Object,
                _mockHttpJourneyService.Object,
                _mockLocalizationHelper.Object,
                _mockConfigSettings.Object);
        }

        [TestMethod]
        public async Task GetWasteTypesViewModel_NoIdParameter_DoesNotCallForWasteCategory()
        {
            // arrange
            _mockHttpWasteService.Setup(s => s.GetWasteMaterialTypes()).ReturnsAsync(new Dictionary<int, string>
            {
                { 2, "" },
                { 4, "" },
                { 7, "" },
                { 9, "" }
            });

            // act
            var result = await _wasteService.GetWasteTypesViewModel(null);

            // assert
            _mockHttpJourneyService.Verify(s => 
                s.GetCategory(
                    It.IsAny<int>()), 
                Times.Never);
            _mockHttpWasteService.Verify(s =>
                s.GetWasteMaterialTypes(), Times.Once);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetWasteTypesViewModel_WithValidIdParameter_CallsForWasteCategory()
        {
            // arrange
            var id = 45;
            _mockHttpWasteService.Setup(s => s.GetWasteMaterialTypes()).ReturnsAsync(new Dictionary<int, string>
            {
                { 2, "" },
                { 4, "" },
                { 7, "" },
                { 9, "" }
            });

            // act
            var result = await _wasteService.GetWasteTypesViewModel(id);

            // assert
            _mockHttpJourneyService.Verify(s =>
                s.GetCategory(
                    It.Is<int>(p => p == id)),
                Times.Once);
            _mockHttpWasteService.Verify(s =>
                s.GetWasteMaterialTypes(), Times.Once);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [Ignore]
        public async Task GetQuarterForCurrentMonth_ReturnsValidModel_ReprocessedIt()
        {
            // Arrange
            int Id = 3;
            var expectedWhatHaveYouDoneWaste = DoneWaste.ReprocessedIt;
            string material = "testMaterial";

            var expectedQuarter = new Dictionary<int, string>
            {
                { 4, "April" },
                { 5, "May" },
                { 6, "June" }
            };

            DuringWhichMonthReceivedRequestViewModel expectedViewModel = new DuringWhichMonthReceivedRequestViewModel
            {
                Id = Id,
                Quarter = expectedQuarter
            };

            _mockHttpJourneyService.Setup(s => s.GetWhatHaveYouDoneWaste(It.Is<int>(p => p == Id))).ReturnsAsync(expectedWhatHaveYouDoneWaste);
            _mockHttpJourneyService.Setup(ws => ws.GetWasteType(It.Is<int>(p => p == Id))).ReturnsAsync(material);

            foreach (var item in expectedQuarter)
            {
                _mockLocalizationHelper.Setup(lh => lh.GetString($"Month{item.Key}")).Returns(item.Value);
            }

            // Act
            var viewModel = await _wasteService.GetQuarterForCurrentMonth(Id);

            // Assert
            Assert.IsNotNull(viewModel);
            Assert.IsTrue(viewModel.Quarter.Count() == 3);
            Assert.IsNotNull(viewModel.WhatHaveYouDone);
            Assert.IsInstanceOfType(viewModel.WhatHaveYouDone, typeof(DoneWaste));
            Assert.IsTrue(viewModel.WhatHaveYouDone == DoneWaste.ReprocessedIt);
            Assert.AreEqual(4, viewModel.Quarter.ElementAt(0).Key);
            Assert.AreEqual("April", viewModel.Quarter.ElementAt(0).Value);
            Assert.AreEqual(5, viewModel.Quarter.ElementAt(1).Key);
            Assert.AreEqual("May", viewModel.Quarter.ElementAt(1).Value);
            Assert.AreEqual(6, viewModel.Quarter.ElementAt(2).Key);
            Assert.AreEqual("June", viewModel.Quarter.ElementAt(2).Value);
            Assert.AreEqual(Id, viewModel.Id);

            foreach (var item in expectedQuarter)
            {
                _mockLocalizationHelper!.Verify(h => h.GetString(It.Is<string>(p => p == $"Month{item.Key}")), Times.Once());
            }

            _mockHttpJourneyService.Verify(service => service.GetWasteType(It.Is<int>(id => id == Id)), Times.Once());
            _mockHttpJourneyService.Verify(service => service.GetWhatHaveYouDoneWaste(It.Is<int>(id => id == Id)), Times.Once());
        }

        [TestMethod]
        [Ignore]
        public async Task GetQuarterForCurrentMonth_ReturnsValidModel_SentItOn()
        {
            // Arrange
            int Id = 3;
            var expectedWhatHaveYouDoneWaste = DoneWaste.SentItOn;
            string material = "testMaterial";

            var expectedQuarter = new Dictionary<int, string>
            {
                { 4, "April" },
                { 5, "May" },
                { 6, "June" }
            };

            DuringWhichMonthSentOnRequestViewModel expectedViewModel = new DuringWhichMonthSentOnRequestViewModel
            {
                Id = Id,
                Quarter = expectedQuarter
            };

            _mockHttpJourneyService.Setup(s => s.GetWhatHaveYouDoneWaste(It.Is<int>(p => p == Id))).ReturnsAsync(expectedWhatHaveYouDoneWaste);
            _mockHttpJourneyService.Setup(ws => ws.GetWasteType(It.Is<int>(p => p == Id))).ReturnsAsync(material);

            foreach (var item in expectedQuarter)
            {
                _mockLocalizationHelper.Setup(lh => lh.GetString($"Month{item.Key}")).Returns(item.Value);
            }

            // Act
            var viewModel = await _wasteService.GetQuarterForCurrentMonth(Id);

            // Assert
            Assert.IsNotNull(viewModel);
            Assert.IsTrue(viewModel.Quarter.Count() == 3);
            Assert.IsNotNull(viewModel.WhatHaveYouDone);
            Assert.IsInstanceOfType(viewModel.WhatHaveYouDone, typeof(DoneWaste));
            Assert.IsTrue(viewModel.WhatHaveYouDone == DoneWaste.SentItOn);
            Assert.AreEqual(4, viewModel.Quarter.ElementAt(0).Key);
            Assert.AreEqual("April", viewModel.Quarter.ElementAt(0).Value);
            Assert.AreEqual(5, viewModel.Quarter.ElementAt(1).Key);
            Assert.AreEqual("May", viewModel.Quarter.ElementAt(1).Value);
            Assert.AreEqual(6, viewModel.Quarter.ElementAt(2).Key);
            Assert.AreEqual("June", viewModel.Quarter.ElementAt(2).Value);
            Assert.AreEqual(Id, viewModel.Id);

            foreach (var item in expectedQuarter)
            {
                _mockLocalizationHelper!.Verify(h => h.GetString(It.Is<string>(p => p == $"Month{item.Key}")), Times.Once());
            }

            _mockHttpJourneyService.Verify(service => service.GetWasteType(It.Is<int>(id => id == Id)), Times.Once());
            _mockHttpJourneyService.Verify(service => service.GetWhatHaveYouDoneWaste(It.Is<int>(id => id == Id)), Times.Once());
        }

        [TestMethod]
        public async Task SaveSelectedWasteType_Succeeds_WithValidModel()
        {
            // Arrange
            var wasteTypesViewModel = new WasteTypeViewModel
            {
                Id = 1,
                MaterialId = 10,
            };

            // Act
            await _wasteService.SaveSelectedWasteType(wasteTypesViewModel);

            // Assert
            _mockHttpJourneyService.Verify(s => s.SaveSelectedWasteType(
                It.Is<int>(p => p == 1), // check that parameter1 (Id) is 1
                It.Is<int>(p => p == 10)) // check that parameter2 (selected waste type id) is 10
            );
        }

        [TestMethod]
        public async Task SaveSelectedMonth_Succeeds_WithValidModel()
        {
            // Arrange
            var duringWhichMonthRequestViewModel = new DuringWhichMonthRequestViewModel
            {
                Id = 1,
                SelectedMonth = 10
            };

            // Act
            await _wasteService.SaveSelectedMonth(duringWhichMonthRequestViewModel);

            // Assert
            _mockHttpJourneyService.Verify(s => s.SaveSelectedMonth(
                It.Is<int>(p => p == 1), // check that parameter1 (Id) is 1
                It.Is<int>(p => p == 10)) // check that parameter3 (What have you done with the waste) is ReprocessedIt
            );
        }

        [TestMethod]
        public async Task SaveSelectedWasteType_ThrowsException_WhenViewModelIsNull()
        {
            // Arrange

            // Act

            // Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await _wasteService.SaveSelectedWasteType(null));
            Assert.AreEqual("Value cannot be null. (Parameter 'wasteTypesViewModel')", exception.Message);
        }

        [TestMethod]
        public async Task SaveSelectedMonth_ThrowsException_WhenViewModelIsNull()
        {
            // Arrange

            // Act

            // Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await _wasteService.SaveSelectedMonth(null));
            Assert.AreEqual("Value cannot be null. (Parameter 'duringWhichMonthRequestViewModel')", exception.Message);
        }

        [TestMethod]
        public async Task SaveSelectedMonth_ThrowsException_WhenSelectedMonthIsNull()
        {
            // Arrange
            var duringWhichMonthRequuestViewModel = new DuringWhichMonthRequestViewModel();
            duringWhichMonthRequuestViewModel.Id = 1;

            // Act

            // Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await _wasteService.SaveSelectedMonth(duringWhichMonthRequuestViewModel));
            Assert.AreEqual("Value cannot be null. (Parameter 'SelectedMonth')", exception.Message);
        }

        [TestMethod]
        public async Task SaveWhatHaveYouDoneWaste_Succeeds_WithValidModel()
        {
            // Arrange
            WhatHaveYouDoneWasteModel whatHaveYouDoneWasteModel = new WhatHaveYouDoneWasteModel();
            whatHaveYouDoneWasteModel.Id = 1;
            whatHaveYouDoneWasteModel.WhatHaveYouDone = DoneWaste.ReprocessedIt;


            // Act
            await _wasteService.SaveWhatHaveYouDoneWaste(whatHaveYouDoneWasteModel);

            // Assert
            _mockHttpJourneyService.Verify(s => s.SaveWhatHaveYouDoneWaste(
                It.Is<int>(p => p == 1),
                It.Is<DoneWaste>(p => p == DoneWaste.ReprocessedIt)
            ));
        }

        [TestMethod]
        public async Task SaveWhatHaveYouDoneWaste_ThrowsException_WhenWasteIsNull()
        {
            // Arrange
            WhatHaveYouDoneWasteModel whatHaveYouDoneWasteModel = new WhatHaveYouDoneWasteModel();
            whatHaveYouDoneWasteModel.Id = 1;
            whatHaveYouDoneWasteModel.WhatHaveYouDone = null;

            // Act

            // Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await _wasteService.SaveWhatHaveYouDoneWaste(whatHaveYouDoneWasteModel));
            Assert.AreEqual("Value cannot be null. (Parameter 'WhatHaveYouDone')", exception.Message);
        }


        [TestMethod]
        public async Task GetWasteRecordStatus_ReturnsDto_WhenValidId()
        {
            // Arrange
            int Id = 1;
            var dto = new WasteRecordStatusDto { WasteRecordStatus = WasteRecordStatuses.Complete };

            _mockHttpJourneyService.Setup(s => s.GetWasteRecordStatus(Id)).ReturnsAsync(dto);

            // Act
            var viewModel = await _wasteService.GetWasteRecordStatus(Id);

            // Assert
            _mockMapper.Verify(m => m.Map<WasteRecordStatusViewModel>(It.Is<WasteRecordStatusDto>(p => p == dto)), Times.Exactly(1));
            _mockHttpJourneyService.Verify(x => x.GetWasteRecordStatus(
                It.Is<int>(i => i == 1)
                ));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task GetWasteRecordStatus_ThrowsException_WhenInvalidId()
        {
            // Arrange
            int Id = -1;

            _mockHttpJourneyService.Setup(s => s.GetWasteRecordStatus(Id)).Throws(new Exception());

            // Act
            var viewModel = await _wasteService.GetWasteRecordStatus(Id);
        }

        [TestMethod]
        public async Task SaveTonnage_WithValidModel_RequestsSavingOfData()
        {
            // arrange
            var exportTonnageViewModel = new ExportTonnageViewModel
            {
                Id = 7,
                ExportTonnes = 56
            };

            // act
            await _wasteService.SaveTonnage(exportTonnageViewModel);

            // assert
            _mockHttpJourneyService.Verify(s =>
                s.SaveTonnage(
                    It.Is<int>(p => p == exportTonnageViewModel.Id),
                    It.Is<double>(p => p == exportTonnageViewModel.ExportTonnes.Value)),
                Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task SaveTonnage_NullParameter_ThrowsNullReferenceException()
        {
            // arrange

            // act
            await _wasteService.SaveTonnage((ExportTonnageViewModel)null);

            // assert
            _mockHttpJourneyService.Verify(s => s.SaveTonnage(
                It.IsAny<int>(),
                It.IsAny<double>()), Times.Never);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task SaveTonnage_NullTonnage_ThrowsNullReferenceException()
        {
            // arrange
            var exportTonnageViewModel = new ExportTonnageViewModel();

            // act
            await _wasteService.SaveTonnage(exportTonnageViewModel);

            // assert
            _mockHttpJourneyService.Verify(s => s.SaveTonnage(
                It.IsAny<int>(),
                It.IsAny<double>()), Times.Never);
        }

        [TestMethod]
        public async Task GetNote_ReturnsValidModel()
        {
            // Arrange
            int Id = 3;
            _mockHttpJourneyService.Setup(s => s.GetNote(Id)).ReturnsAsync(new NoteDto());

            // Act
            var viewModel = await _wasteService.GetNoteViewModel(Id);

            // Assert
            Assert.IsNotNull(viewModel);
            Assert.AreEqual(Id, viewModel.Id);

            _mockHttpJourneyService.Verify(service => service.GetNote(It.Is<int>(id => id == Id)), Times.Once());
        }

        [TestMethod]
        public async Task SaveNote_Succeeds_WithValidModel()
        {
            // Arrange
            var noteViewModel = new NoteViewModel
            {
                Id = 1,
                NoteContent = "Some dummy note content"
            };

            // Act
            await _wasteService.SaveNote(noteViewModel);

            // Assert
            _mockHttpJourneyService.Verify(s => s.SaveNote(
                It.Is<int>(p => p == 1), // check that parameter1 (Id) is 1
                It.Is<string>(p => p == "Some dummy note content")) // check that parameter2 (note content) is some dummy note content
            );
        }

        [TestMethod]
        public async Task SaveNote_ThrowsException_WhenViewModelIsNull()
        {
            // Arrange

            // Act

            // Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await _wasteService.SaveNote(null));
            Assert.AreEqual("Value cannot be null. (Parameter 'noteViewModel')", exception.Message);
        }

        #region Baled with wire

        [TestMethod]
        public async Task GetBaledWithWireModel_Succeeds_WithValidModel()
        {
            // Arrange
            var Id = 1;

            // Act
            await _wasteService.GetBaledWithWireModel(Id);

            // Assert
            _mockHttpJourneyService.Verify(s => s.GetBaledWithWire(
                It.Is<int>(p => p == 1)),
                Times.Once);
        }


        [TestMethod]
        public async Task SaveBaledWithWireModel_Succeeds_WithValidModel()
        {
            // Arrange
            BaledWithWireViewModel baledWithWireModel = new BaledWithWireViewModel();
            baledWithWireModel.Id = 1;
            baledWithWireModel.BaledWithWire = true;
            baledWithWireModel.BaledWithWireDeductionPercentage = 2;

            // Act
            await _wasteService.SaveBaledWithWire(baledWithWireModel);

            // Assert
            _mockHttpJourneyService.Verify(s => s.SaveBaledWithWire(
                It.Is<int>(p => p == 1),
                It.Is<bool>(p => p == true),
                It.IsAny<double>()),
                Times.Once);
        }

        [TestMethod]
        public async Task SaveBaledWithWireModel_ThrowsException_WhenWireIsNull()
        {
            // Arrange
            BaledWithWireViewModel baledWithWireModel = new BaledWithWireViewModel();
            baledWithWireModel.Id = 1;
            baledWithWireModel.BaledWithWire = null;

            // Act

            // Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await _wasteService.SaveBaledWithWire(baledWithWireModel));
            Assert.AreEqual("Value cannot be null. (Parameter 'BaledWithWire')", exception.Message);
        }

        #endregion

        #region Category

        [TestMethod]
        [Ignore]
        public async Task GetCategory_Exporter()
        {
            // Arrange
            int Id = 3;
            int currentMonth = 5;
            
            DuringWhichMonthReceivedRequestViewModel expectedViewModel = new DuringWhichMonthReceivedRequestViewModel
            {
                Id = Id,
                Category = Category.Exporter
            };

            _mockHttpJourneyService.Setup(c => c.GetCategory(It.Is<int>(p => p == Id))).ReturnsAsync(Category.Exporter);

            // Act
            var viewModel = await _wasteService.GetQuarterForCurrentMonth(Id);

            // Assert
            Assert.IsNotNull(viewModel);
 
            Assert.AreEqual(Id, viewModel.Id);
            Assert.AreEqual(viewModel.Category, Category.Exporter);

            _mockHttpJourneyService.Verify(service => service.GetCategory(It.Is<int>(id => id == Id)), Times.Once());
        }

        [TestMethod]
        [Ignore]
        public async Task GetCategory_Reprocessor()
        {
            // Arrange
            int Id = 3;
            int currentMonth = 5;

            DuringWhichMonthReceivedRequestViewModel expectedViewModel = new DuringWhichMonthReceivedRequestViewModel
            {
                Id = Id,
                Category = Category.Reprocessor
            };

            _mockHttpJourneyService.Setup(c => c.GetCategory(It.Is<int>(p => p == Id))).ReturnsAsync(Category.Reprocessor);

            // Act
            var viewModel = await _wasteService.GetQuarterForCurrentMonth(Id);

            // Assert
            Assert.IsNotNull(viewModel);

            Assert.AreEqual(Id, viewModel.Id);
            Assert.AreEqual(viewModel.Category, Category.Reprocessor);

            _mockHttpJourneyService.Verify(service => service.GetCategory(It.Is<int>(id => id == Id)), Times.Once());
        }

        #endregion

    }
}
