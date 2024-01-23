using EPRN.Waste.API.Controllers;
using EPRN.Waste.API.Services.Interfaces;
using Moq;

namespace EPRN.UnitTests.API.Waste.Controllers
{
    [TestClass]
    public class WasteControllerTests
    {
        private WasteController _wasteController;
        private Mock<IWasteService> _mockWasteService;
        private Mock<IQuarterlyDatesService> _mockQuarterlyDatesService;
        
        [TestInitialize]
        public void Init()
        {
            _mockWasteService = new Mock<IWasteService>();
            _mockQuarterlyDatesService = new Mock<IQuarterlyDatesService>();
            _wasteController = new WasteController(_mockWasteService.Object);
        }

        [TestMethod]
        public async Task WasteTypes_CallsService_Successfully()
        {
            // arrange

            // act
            await _wasteController.WasteTypes();

            // Assert
            _mockWasteService.Verify(s => s.WasteTypes(), Times.Once());
        }
    }
}
