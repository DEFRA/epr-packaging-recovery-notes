using AutoMapper;
using EPRN.Common.Dtos;
using EPRN.Common.Enums;
using EPRN.Portal.Helpers.Interfaces;
using EPRN.Portal.Resources;
using EPRN.Portal.RESTServices.Interfaces;
using EPRN.Portal.Services;
using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels.Waste;
using Moq;

namespace EPRN.UnitTests.Portal.Services
{
    [TestClass]
    public class WasteServiceTests
    {
        private IWasteService _wasteService = null;
        private Mock<IMapper> _mockMapper = null;
        private Mock<IHttpWasteService> _mockHttpWasteService = null;
        private Mock<IHttpJourneyService> _mockHttpJourneyService = null;
        private Mock<ILocalizationHelper<WhichQuarterResources>> _mockLocalizationHelper = null;

        [TestInitialize]
        public void Init()
        {
            _mockMapper = new Mock<IMapper>();
            _mockHttpWasteService = new Mock<IHttpWasteService>();
            _mockHttpJourneyService = new Mock<IHttpJourneyService>();
            _mockLocalizationHelper = new Mock<ILocalizationHelper<WhichQuarterResources>>();

            _wasteService = new WasteService(
                _mockMapper.Object,
                _mockHttpWasteService.Object,
                _mockHttpJourneyService.Object,
                _mockLocalizationHelper.Object);
        }

        [TestMethod]
        public async Task GetWasteTypesViewModel_ReturnsValidModel_MappedCorrectly()
        {
            // Arrange
            var wasteType = new List<WasteTypeDto>
            {
                new WasteTypeDto
                {
                    Id = 1,
                    Name = "Test1",
                },
                new WasteTypeDto
                {
                    Id = 99,
                    Name = "Test99",
                }
            };

            _mockHttpWasteService.Setup(s => s.GetWasteMaterialTypes()).ReturnsAsync(wasteType);

            // Act
            var viewModel = await _wasteService.GetWasteTypesViewModel(7);

            // Assert
            Assert.IsNotNull(viewModel);
            Assert.IsTrue(viewModel.WasteTypes.Count() == 2);
            Assert.AreEqual(1, viewModel.WasteTypes.ElementAt(0).Key);
            Assert.AreEqual("Test1", viewModel.WasteTypes.ElementAt(0).Value);
            Assert.AreEqual(99, viewModel.WasteTypes.ElementAt(1).Key);
            Assert.AreEqual("Test99", viewModel.WasteTypes.ElementAt(1).Value);
        }

        [TestMethod]
        public async Task GetQuarterForCurrentMonth_ReturnsValidModel_ReprocessedIt()
        {
            // Arrange
            int journeyId = 3;
            int currentMonth = 5;
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
                JourneyId = journeyId,
                Quarter = expectedQuarter
            };

            _mockHttpJourneyService.Setup(s => s.GetWhatHaveYouDoneWaste(It.Is<int>(p => p == journeyId))).ReturnsAsync(expectedWhatHaveYouDoneWaste);
            _mockHttpJourneyService.Setup(ws => ws.GetWasteType(It.Is<int>(p => p == journeyId))).ReturnsAsync(material);

            foreach (var item in expectedQuarter)
            {
                _mockLocalizationHelper.Setup(lh => lh.GetString($"Month{item.Key}")).Returns(item.Value);
            }

            // Act
            var viewModel = await _wasteService.GetQuarterForCurrentMonth(journeyId, currentMonth);

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
            Assert.AreEqual(material, viewModel.WasteType);
            Assert.AreEqual(journeyId, viewModel.JourneyId);

            foreach (var item in expectedQuarter)
            {
                _mockLocalizationHelper!.Verify(h => h.GetString(It.Is<string>(p => p == $"Month{item.Key}")), Times.Once());
            }

            _mockHttpJourneyService.Verify(service => service.GetWasteType(It.Is<int>(id => id == journeyId)), Times.Once());
            _mockHttpJourneyService.Verify(service => service.GetWhatHaveYouDoneWaste(It.Is<int>(id => id == journeyId)), Times.Once());
        }

        [TestMethod]
        public async Task GetQuarterForCurrentMonth_ReturnsValidModel_SentItOn()
        {
            // Arrange
            int journeyId = 3;
            int currentMonth = 5;
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
                JourneyId = journeyId,
                Quarter = expectedQuarter
            };

            _mockHttpJourneyService.Setup(s => s.GetWhatHaveYouDoneWaste(It.Is<int>(p => p == journeyId))).ReturnsAsync(expectedWhatHaveYouDoneWaste);
            _mockHttpJourneyService.Setup(ws => ws.GetWasteType(It.Is<int>(p => p == journeyId))).ReturnsAsync(material);

            foreach (var item in expectedQuarter)
            {
                _mockLocalizationHelper.Setup(lh => lh.GetString($"Month{item.Key}")).Returns(item.Value);
            }

            // Act
            var viewModel = await _wasteService.GetQuarterForCurrentMonth(journeyId, currentMonth);

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
            Assert.AreEqual(material, viewModel.WasteType);
            Assert.AreEqual(journeyId, viewModel.JourneyId);

            foreach (var item in expectedQuarter)
            {
                _mockLocalizationHelper!.Verify(h => h.GetString(It.Is<string>(p => p == $"Month{item.Key}")), Times.Once());
            }

            _mockHttpJourneyService.Verify(service => service.GetWasteType(It.Is<int>(id => id == journeyId)), Times.Once());
            _mockHttpJourneyService.Verify(service => service.GetWhatHaveYouDoneWaste(It.Is<int>(id => id == journeyId)), Times.Once());
        }

        [TestMethod]
        public async Task SaveSelectedWasteType_Succeeds_WithValidModel()
        {
            // Arrange
            var wasteTypesViewModel = new WasteTypesViewModel
            {
                JourneyId = 1,
                SelectedWasteTypeId = 10,
            };

            // Act
            await _wasteService.SaveSelectedWasteType(wasteTypesViewModel);

            // Assert
            _mockHttpJourneyService.Verify(s => s.SaveSelectedWasteType(
                It.Is<int>(p => p == 1), // check that parameter1 (journeyId) is 1
                It.Is<int>(p => p == 10)) // check that parameter2 (selected waste type id) is 10
            );
        }

        [TestMethod]
        public async Task SaveSelectedMonth_Succeeds_WithValidModel()
        {
            // Arrange
            var duringWhichMonthRequestViewModel = new DuringWhichMonthRequestViewModel
            {
                JourneyId = 1,
                SelectedMonth = 10
            };

            // Act
            await _wasteService.SaveSelectedMonth(duringWhichMonthRequestViewModel);

            // Assert
            _mockHttpJourneyService.Verify(s => s.SaveSelectedMonth(
                It.Is<int>(p => p == 1), // check that parameter1 (journeyId) is 1
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
        public async Task SaveSelectedWasteType_ThrowsException_WhenSelectedWasteTypeIdIsNull()
        {
            // Arrange
            var wasteTypesViewModel = new WasteTypesViewModel();

            // Act

            // Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await _wasteService.SaveSelectedWasteType(wasteTypesViewModel));
            Assert.AreEqual("Value cannot be null. (Parameter 'SelectedWasteTypeId')", exception.Message);
        }

        [TestMethod]
        public async Task SaveSelectedMonth_ThrowsException_WhenSelectedMonthIsNull()
        {
            // Arrange
            var duringWhichMonthRequuestViewModel = new DuringWhichMonthRequestViewModel();
            duringWhichMonthRequuestViewModel.JourneyId = 1;

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
            whatHaveYouDoneWasteModel.JourneyId = 1;
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
            whatHaveYouDoneWasteModel.JourneyId = 1;
            whatHaveYouDoneWasteModel.WhatHaveYouDone = null;

            // Act

            // Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await _wasteService.SaveWhatHaveYouDoneWaste(whatHaveYouDoneWasteModel));
            Assert.AreEqual("Value cannot be null. (Parameter 'WhatHaveYouDone')", exception.Message);
        }


        [TestMethod]
        public async Task GetWasteRecordStatus_ReturnsDto_WhenValidJourneyId()
        {
            // Arrange
            int journeyId = 1;
            var dto = new WasteRecordStatusDto { WasteRecordStatus = WasteRecordStatuses.Complete };

            _mockHttpJourneyService.Setup(s => s.GetWasteRecordStatus(journeyId)).ReturnsAsync(dto);

            // Act
            var viewModel = await _wasteService.GetWasteRecordStatus(journeyId);

            // Assert
            _mockMapper.Verify(m => m.Map<WasteRecordStatusViewModel>(It.Is<WasteRecordStatusDto>(p => p == dto)), Times.Exactly(1));
            _mockHttpJourneyService.Verify(x => x.GetWasteRecordStatus(
                It.Is<int>(i => i == 1)
                ));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task GetWasteRecordStatus_ThrowsException_WhenInvalidJourneyId()
        {
            // Arrange
            int journeyId = -1;

            _mockHttpJourneyService.Setup(s => s.GetWasteRecordStatus(journeyId)).Throws(new Exception());

            // Act
            var viewModel = await _wasteService.GetWasteRecordStatus(journeyId);
        }

        [TestMethod]
        public async Task SaveTonnage_WithValidModel_RequestsSavingOfData()
        {
            // arrange
            var exportTonnageViewModel = new ExportTonnageViewModel
            {
                JourneyId = 7,
                ExportTonnes = 56
            };

            // act
            await _wasteService.SaveTonnage(exportTonnageViewModel);

            // assert
            _mockHttpJourneyService.Verify(s =>
                s.SaveTonnage(
                    It.Is<int>(p => p == exportTonnageViewModel.JourneyId),
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
            int journeyId = 3;

            // Act
            var viewModel = await _wasteService.GetNoteViewModel(journeyId);

            // Assert
            Assert.IsNotNull(viewModel);
            Assert.AreEqual(journeyId, viewModel.JourneyId);

            _mockHttpJourneyService.Verify(service => service.GetNote(It.Is<int>(id => id == journeyId)), Times.Once());
        }

        [TestMethod]
        public async Task SaveNote_Succeeds_WithValidModel()
        {
            // Arrange
            var noteViewModel = new NoteViewModel
            {
                JourneyId = 1,
                WasteType = "testWasteType",
                NoteContent = "Some dummy note content"
            };

            // Act
            await _wasteService.SaveNote(noteViewModel);

            // Assert
            _mockHttpJourneyService.Verify(s => s.SaveNote(
                It.Is<int>(p => p == 1), // check that parameter1 (journeyId) is 1
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
            var journeyId = 1;

            // Act
            await _wasteService.GetBaledWithWireModel(journeyId);

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
            baledWithWireModel.JourneyId = 1;
            baledWithWireModel.BaledWithWire = true;


            // Act
            await _wasteService.SaveBaledWithWire(baledWithWireModel);

            // Assert
            _mockHttpJourneyService.Verify(s => s.SaveBaledWithWire(
                It.Is<int>(p => p == 1),
                It.Is<bool>(p => p == true),
                It.Is<double>(p => p == 2)),
                Times.Once);
        }

        [TestMethod]
        public async Task SaveBaledWithWireModel_ThrowsException_WhenWireIsNull()
        {
            // Arrange
            BaledWithWireViewModel baledWithWireModel = new BaledWithWireViewModel();
            baledWithWireModel.JourneyId = 1;
            baledWithWireModel.BaledWithWire = null;

            // Act

            // Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await _wasteService.SaveBaledWithWire(baledWithWireModel));
            Assert.AreEqual("Value cannot be null. (Parameter 'BaledWithWire')", exception.Message);
        }

        #endregion

    }
}
