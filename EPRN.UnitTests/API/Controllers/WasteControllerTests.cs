using EPRN.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Waste.API.Controllers;
using Waste.API.Services.Interfaces;

namespace EPRN.UnitTests.API.Controllers
{
    [TestClass]
    public class WasteControllerTests
    {
        private WasteController? _wasteController;
        private Mock<IWasteService>? _mockWasteService;

        [TestInitialize]
        public void Init()
        {
            _mockWasteService = new Mock<IWasteService>();
            _wasteController = new WasteController(_mockWasteService.Object);
        }

        [TestMethod]
        public async Task TestWasteType_Returns_OK()
        {
            //Arrange
            var journeyId = 3;

            //Act
            var result = await _wasteController!.WasteType(journeyId);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            _mockWasteService.Verify(s => s.GetWasteType(
                 It.Is<int>(p => p == journeyId))
             );
        }

        [TestMethod]
        public async Task TestWasteType_ReturnsBadRequest_WhenNoIdSupplied()
        {
            //Arrange

            //Act
            var result = await _wasteController!.WasteType(null);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            _mockWasteService.Verify(s => s.GetWasteType(
                 It.Is<int>(p => p == null)), Times.Never
             );
        }

        [TestMethod]
        public async Task TestSaveJourneyMonth_ReturnsOk_WhenValid()
        {
            //Arrange
            var journeyId = 5;
            var monthSelected = 7;
            _mockWasteService!.Setup(ws => ws.CreateJourney()).ReturnsAsync(journeyId);

            //Act
            var result = await _wasteController!.SaveJourneyMonth(journeyId, monthSelected);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkResult));
            _mockWasteService.Verify(s => s.SaveSelectedMonth(
                 It.Is<int>(p => p == journeyId),
                 It.Is<int>(p => p == monthSelected)), Times.Once
             );
        }

        [TestMethod]
        public async Task TestSaveJourneyMonth_ReturnsBadRequest_WhenNoJourneyId()
        {
            //Arrange
            var monthSelected = 7;
            _mockWasteService!.Setup(ws => ws.CreateJourney());

            //Act
            var result = await _wasteController!.SaveJourneyMonth(null, monthSelected);

            //Assert
            Assert.AreEqual(monthSelected, 7);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            _mockWasteService.Verify(s => s.SaveSelectedMonth(
                 It.Is<int>(p => p == null),
                 It.Is<int>(p => p == monthSelected)), Times.Never
             );
        }

        [TestMethod]
        public async Task TestSaveJourneyMonth_ReturnsBadRequest_WhenNoMonthSelected()
        {
            //Arrange
            var journeyId = 5;
            _mockWasteService!.Setup(ws => ws.CreateJourney());

            //Act
            var result = await _wasteController!.SaveJourneyMonth(journeyId, null);

            //Assert
            Assert.AreEqual(journeyId, 5);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            _mockWasteService.Verify(s => s.SaveSelectedMonth(
                 It.Is<int>(p => p == journeyId),
                 It.Is<int>(p => p == null)), Times.Never
             );
        }

        [TestMethod]
        public async Task TestSaveWhatHaveYouDoneWaste_ReturnsOk_WhenValid()
        {
            //Arrange
            var journeyId = 5;
            _mockWasteService!.Setup(ws => ws.CreateJourney()).ReturnsAsync(journeyId);

            //Act
            var result = await _wasteController!.SaveWhatHaveYouDoneWaste(journeyId, DoneWaste.ReprocessedIt);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            _mockWasteService.Verify(s => s.SaveWhatHaveYouDoneWaste(
                 It.Is<int>(p => p == journeyId),
                 It.Is<DoneWaste>(p => p == DoneWaste.ReprocessedIt)), Times.Once
             );
        }

        [TestMethod]
        public async Task TestSaveWhatHaveYouDoneWaste_ReturnsBadRequest_WhenNoJourneyId()
        {
            //Arrange
            _mockWasteService!.Setup(ws => ws.CreateJourney());

            //Act
            var result = await _wasteController!.SaveWhatHaveYouDoneWaste(null, DoneWaste.ReprocessedIt);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            _mockWasteService.Verify(s => s.SaveWhatHaveYouDoneWaste(
                 It.Is<int>(p => p == null),
                 It.Is<DoneWaste>(p => p == DoneWaste.ReprocessedIt)), Times.Never
             );
        }


    }
}
