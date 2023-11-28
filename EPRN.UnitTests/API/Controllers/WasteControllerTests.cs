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
        }

        [TestMethod]
        public async Task TestSaveJourneyMonth_ReturnsOk_WhenValid()
        {
            //Arrange
            var journeyId = 5;
            var monthSelected = 7;
            _mockWasteService!.Setup(ws => ws.CreateJourney());

            //Act
            var result = await _wasteController!.SaveJourneyMonth(journeyId, monthSelected);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkResult));
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
        }
    }
}
