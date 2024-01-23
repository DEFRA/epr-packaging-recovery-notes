using EPRN.Common.Enums;
using EPRN.Waste.API.Controllers;
using EPRN.Waste.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EPRN.UnitTests.API.Waste.Controllers
{
    [TestClass]
    public class JourneyControllerTests
    {
        private JourneyController _journeyController;
        private Mock<IJourneyService> _mockJourneyService;

        [TestInitialize]
        public void Init()
        {
            _mockJourneyService = new Mock<IJourneyService>();
            var mockQuarterlyDatesService = new Mock<IQuarterlyDatesService>();
            _journeyController = new JourneyController(_mockJourneyService.Object, mockQuarterlyDatesService.Object);
        }

        [TestMethod]
        public async Task CreateJourney_CallsService()
        {
            // arrange
            
            // act
            await _journeyController.CreateJourney();

            // assert
            _mockJourneyService.Verify(s => s.CreateJourney(), Times.Once());
        }

        [TestMethod]
        public async Task WasteType_Returns_OK()
        {
            //Arrange
            var journeyId = 3;

            //Act
            var result = await _journeyController!.WasteType(journeyId);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            _mockJourneyService.Verify(s => s.GetWasteType(
                 It.Is<int>(p => p == journeyId))
             );
        }

        [TestMethod]
        public async Task WasteType_ReturnsBadRequest_WhenNoIdSupplied()
        {
            //Arrange

            //Act
            var result = await _journeyController!.WasteType(null);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            _mockJourneyService.Verify(s => s.GetWasteType(
                 It.IsAny<int>()), Times.Never
             );
        }

        [TestMethod]
        public async Task SaveJourneyMonth_ReturnsOk_WhenValid()
        {
            //Arrange
            var journeyId = 5;
            var monthSelected = 7;

            _mockJourneyService!.Setup(ws => ws.CreateJourney()).ReturnsAsync(journeyId);

            //Act
            var result = await _journeyController!.SaveJourneyMonth(journeyId, monthSelected);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkResult));
            _mockJourneyService.Verify(s => s.SaveSelectedMonth(
                 It.Is<int>(p => p == journeyId),
                 It.Is<int>(p => p == monthSelected)), Times.Once
             );
        }

        [TestMethod]
        public async Task SaveJourneyMonth_ReturnsBadRequest_WhenNoJourneyId()
        {
            //Arrange
            var monthSelected = 7;

            _mockJourneyService!.Setup(ws => ws.CreateJourney());

            //Act
            var result = await _journeyController.SaveJourneyMonth(null, monthSelected);

            //Assert
            Assert.AreEqual(monthSelected, 7);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            _mockJourneyService.Verify(s => s.SaveSelectedMonth(
                 It.IsAny<int>(),
                 It.Is<int>(p => p == monthSelected)), Times.Never
             );
        }

        [TestMethod]
        public async Task SaveJourneyMonth_ReturnsBadRequest_WhenNoMonthSelected()
        {
            //Arrange
            var journeyId = 5;
            var expectedWhatHaveYouDoneWaste = DoneWaste.SentItOn;

            _mockJourneyService.Setup(ws => ws.CreateJourney());
            _mockJourneyService.Setup(ws => ws.GetWhatHaveYouDoneWaste(It.Is<int>(p => p == journeyId))).ReturnsAsync(expectedWhatHaveYouDoneWaste);

            //Act
            var result = await _journeyController.SaveJourneyMonth(journeyId, null);

            //Assert
            Assert.AreEqual(journeyId, 5);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            _mockJourneyService.Verify(s => s.SaveSelectedMonth(
                 It.Is<int>(p => p == journeyId),
                 It.IsAny<int>()), Times.Never
             );
        }

        [TestMethod]
        public async Task SaveWhatHaveYouDoneWaste_ReturnsOk_WhenValid()
        {
            //Arrange
            var journeyId = 5;
            _mockJourneyService!.Setup(ws => ws.CreateJourney()).ReturnsAsync(journeyId);

            //Act
            var result = await _journeyController!.SaveWhatHaveYouDoneWaste(journeyId, DoneWaste.ReprocessedIt);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkResult));
            _mockJourneyService.Verify(s => s.SaveWhatHaveYouDoneWaste(
                 It.Is<int>(p => p == journeyId),
                 It.Is<DoneWaste>(p => p == DoneWaste.ReprocessedIt)), Times.Once
             );
        }

        [TestMethod]
        public async Task SaveWhatHaveYouDoneWaste_ReturnsBadRequest_WhenNoJourneyId()
        {
            //Arrange
            _mockJourneyService!.Setup(ws => ws.CreateJourney());

            //Act
            var result = await _journeyController.SaveWhatHaveYouDoneWaste(null, DoneWaste.ReprocessedIt);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            _mockJourneyService.Verify(s => s.SaveWhatHaveYouDoneWaste(
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
            var result = await _journeyController.SetJourneyTonnage(journeyId, tonnage);

            // assert
            _mockJourneyService.Verify(s =>
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
            var result = await _journeyController.SetJourneyTonnage(null, tonnage);

            // assert
            _mockJourneyService.Verify(s =>
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
            var result = await _journeyController.SetJourneyTonnage(journeyId, null);

            // assert
            _mockJourneyService.Verify(s =>
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
            var result = await _journeyController.GetWhatHaveYouDoneWaste(journeyId);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            _mockJourneyService.Verify(s => s.GetWhatHaveYouDoneWaste(It.Is<int>(p => p == journeyId)));
        }

        [TestMethod]
        public async Task GetWhatHaveYouDoneWaste_ReturnsBadRequest_WhenNoIdSupplied()
        {
            //Arrange

            //Act
            var result = await _journeyController.GetWhatHaveYouDoneWaste(null);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            _mockJourneyService.Verify(s => s.GetWhatHaveYouDoneWaste(It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public async Task GetGetBaledWithWire_Returns_OK()
        {
            //Arrange
            var journeyId = 3;

            //Act
            var result = await _journeyController.GetBaledWithWire(journeyId);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            _mockJourneyService.Verify(s => s.GetBaledWithWire(It.Is<int>(p => p == journeyId)));
        }

        [TestMethod]
        public async Task GetGetBaledWithWire_ReturnsBadRequest_WhenNoIdSupplied()
        {
            //Arrange

            //Act
            var result = await _journeyController.GetBaledWithWire(null);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            _mockJourneyService.Verify(s => s.GetBaledWithWire(It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public async Task GetCategory_Returns_OK()
        {
            //Arrange
            var journeyId = 3;

            //Act
            var result = await _journeyController.GetJourneyCategory(journeyId);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            _mockJourneyService.Verify(s => s.GetCategory(It.Is<int>(p => p == journeyId)));
        }

        [TestMethod]
        public async Task GetCategory_Returns_OK_ReturnsBadRequest_WhenNoIdSupplied()
        {
            //Arrange

            //Act
            var result = await _journeyController.GetJourneyCategory(null);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            _mockJourneyService.Verify(s => s.GetCategory(It.IsAny<int>()), Times.Never);
        }
    }
}
