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
        private WasteController _wasteController;
        private Mock<IWasteService> _mockWasteService;

        [TestInitialize]
        public void Init()
        {
            _mockWasteService = new Mock<IWasteService>();
            _wasteController = new WasteController(_mockWasteService.Object);
        }

        [TestMethod]
        public async Task WasteType_Returns_OK()
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
        public async Task WasteType_ReturnsBadRequest_WhenNoIdSupplied()
        {
            //Arrange

            //Act
            var result = await _wasteController!.WasteType(null);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            _mockWasteService.Verify(s => s.GetWasteType(
                 It.IsAny<int>()), Times.Never
             );
        }

        [TestMethod]
        public async Task SaveJourneyMonth_ReturnsOk_WhenValid()
        {
            //Arrange
            var journeyId = 5;
            var monthSelected = 7;
            var whatHaveYouDoneWaste = DoneWaste.ReprocessedIt;

            _mockWasteService!.Setup(ws => ws.CreateJourney()).ReturnsAsync(journeyId);

            //Act
            var result = await _wasteController!.SaveJourneyMonth(journeyId, monthSelected, whatHaveYouDoneWaste);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkResult));
            _mockWasteService.Verify(s => s.SaveSelectedMonth(
                 It.Is<int>(p => p == journeyId),
                 It.Is<int>(p => p == monthSelected),
                 It.Is<DoneWaste>(p => p == whatHaveYouDoneWaste)), Times.Once
             );
        }

        [TestMethod]
        public async Task SaveJourneyMonth_ReturnsBadRequest_WhenNoJourneyId()
        {
            //Arrange
            var monthSelected = 7;
            var whatHaveYouDoneWaste = DoneWaste.ReprocessedIt;

            _mockWasteService!.Setup(ws => ws.CreateJourney());

            //Act
            var result = await _wasteController!.SaveJourneyMonth(null, monthSelected, whatHaveYouDoneWaste);

            //Assert
            Assert.AreEqual(monthSelected, 7);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            _mockWasteService.Verify(s => s.SaveSelectedMonth(
                 It.IsAny<int>(),
                 It.Is<int>(p => p == monthSelected),
                 It.Is<DoneWaste>(p => p == whatHaveYouDoneWaste)), Times.Never
             );
        }

        [TestMethod]
        public async Task SaveJourneyMonth_ReturnsBadRequest_WhenNoMonthSelected()
        {
            //Arrange
            var journeyId = 5;
            var whatHaveYouDoneWaste = DoneWaste.ReprocessedIt;

            _mockWasteService!.Setup(ws => ws.CreateJourney());

            //Act
            var result = await _wasteController!.SaveJourneyMonth(journeyId, null, whatHaveYouDoneWaste);

            //Assert
            Assert.AreEqual(journeyId, 5);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            _mockWasteService.Verify(s => s.SaveSelectedMonth(
                 It.Is<int>(p => p == journeyId),
                 It.IsAny<int>(),
                 It.Is<DoneWaste>(p => p == whatHaveYouDoneWaste)), Times.Never
             );
        }

        [TestMethod]
        public async Task SaveWhatHaveYouDoneWaste_ReturnsOk_WhenValid()
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
        public async Task SaveWhatHaveYouDoneWaste_ReturnsBadRequest_WhenNoJourneyId()
        {
            //Arrange
            _mockWasteService!.Setup(ws => ws.CreateJourney());

            //Act
            var result = await _wasteController.SaveWhatHaveYouDoneWaste(null, DoneWaste.ReprocessedIt);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            _mockWasteService.Verify(s => s.SaveWhatHaveYouDoneWaste(
                 It.IsAny<int>(),
                 It.Is<DoneWaste>(p => p == DoneWaste.ReprocessedIt)), Times.Never
             );
        }

        [TestMethod]
        public async Task SetJourneyTonnage_WithValidParameters_SavesSuccesfully()
        {
            // arrange
            int journeyId = 4;
            double tonnage = 56.7;

            // act
            var result = await _wasteController.SetJourneyTonnage(journeyId, tonnage);

            // assert
            _mockWasteService.Verify(s =>
                s.SaveTonnage(
                    It.Is<int>(p => p == journeyId),
                    It.Is<double>(p => p == tonnage)),
                Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task SetJourneyTonnage_WithNoJourneyID_ThrowsBadRequest()
        {
            // arrange
            double tonnage = 56.7;

            // act
            var result = await _wasteController.SetJourneyTonnage(null, tonnage);

            // assert
            _mockWasteService.Verify(s =>
                s.SaveTonnage(
                    It.IsAny<int>(),
                    It.IsAny<double>()),
                Times.Never);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task SetJourneyTonnage_WithNoTonnage_ThrowsBadRequest()
        {
            // arrange
            int journeyId = 4;

            // act
            var result = await _wasteController.SetJourneyTonnage(journeyId, null);

            // assert
            _mockWasteService.Verify(s =>
                s.SaveTonnage(
                    It.IsAny<int>(),
                    It.IsAny<double>()),
                Times.Never);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task GetWhatHaveYouDoneWaste_Returns_OK()
        {
            //Arrange
            var journeyId = 3;

            //Act
            var result = await _wasteController.GetWhatHaveYouDoneWaste(journeyId);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            _mockWasteService.Verify(s => s.GetWhatHaveYouDoneWaste(It.Is<int>(p => p == journeyId)));
        }

        [TestMethod]
        public async Task GetWhatHaveYouDoneWaste_ReturnsBadRequest_WhenNoIdSupplied()
        {
            //Arrange

            //Act
            var result = await _wasteController.GetWhatHaveYouDoneWaste(null);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            _mockWasteService.Verify(s => s.GetWhatHaveYouDoneWaste(It.IsAny<int>()), Times.Never);
        }
    }
}
