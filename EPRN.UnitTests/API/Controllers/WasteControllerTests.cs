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
        public async Task TestWasteType_Returns_OK()
        {
            //Arrange
            var journeyId = 3;

            //Act
            var result = await _wasteController.WasteType(journeyId);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task TestWasteType_ReturnsBadRequest_WhenNoIdSupplied()
        {
            //Arrange

            //Act
            var result = await _wasteController.WasteType(null);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }
    }
}
