using AutoMapper;
using EPRN.Common.Dtos;
using Moq;
using Portal.RESTServices.Interfaces;
using Portal.Services;
using Portal.Services.Interfaces;
using Portal.ViewModels;

namespace EPRN.UnitTests.Portal.Services
{
    [TestClass]
    public class WasteServiceTests
    {
        private IWasteService? _wasteService = null;
        private Mock<IMapper>? _mockMapper = null;
        private Mock<IHttpWasteService>? _mockHttpWasteService = null;

        [TestInitialize]
        public void Init()
        {
            _mockMapper = new Mock<IMapper>();
            _mockHttpWasteService = new Mock<IHttpWasteService>();

            _wasteService = new WasteService(
                _mockMapper.Object,
                _mockHttpWasteService.Object);
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
        public async Task GetCurrentQuarter_ReturnsValidModel()
        {
            // Arrange
            int journeyId = 3;
            string material = "testMaterial";
            _mockHttpWasteService.Setup(ws => ws.GetWasteType(It.Is<int>(p => p == journeyId))).ReturnsAsync(material);

            // Act
            var viewModel = await _wasteService.GetCurrentQuarter(journeyId);

            // Assert
            Assert.IsNotNull(viewModel);
            Assert.IsTrue(viewModel.Quarter.Count() == 3);
            Assert.AreEqual(10, viewModel.Quarter.ElementAt(0).Key);
            Assert.AreEqual("October", viewModel.Quarter.ElementAt(0).Value);
            Assert.AreEqual(11, viewModel.Quarter.ElementAt(1).Key);
            Assert.AreEqual("November", viewModel.Quarter.ElementAt(1).Value);
            Assert.AreEqual(12, viewModel.Quarter.ElementAt(2).Key);
            Assert.AreEqual("December", viewModel.Quarter.ElementAt(2).Value);
            Assert.AreEqual(material, viewModel.WasteType);
            Assert.AreEqual(journeyId, viewModel.JourneyId);
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
            _mockHttpWasteService.Verify(s => s.SaveSelectedWasteType(
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
                SelectedMonth = 10,
            };

            // Act
            await _wasteService.SaveSelectedMonth(duringWhichMonthRequestViewModel);

            // Assert
            _mockHttpWasteService.Verify(s => s.SaveSelectedMonth(
                It.Is<int>(p => p == 1), // check that parameter1 (journeyId) is 1
                It.Is<int>(p => p == 10)) // check that parameter2 (selected month) is 10
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
    }
}
