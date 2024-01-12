using AutoMapper;
using EPRN.Waste.API.Repositories.Interfaces;
using EPRN.Waste.API.Services;
using EPRN.Waste.API.Services.Interfaces;
using Moq;

namespace EPRN.UnitTests.API.Waste.Services
{
    [TestClass]
    public class WasteServiceTests
    {
        private IWasteService _wasteService;
        private Mock<IMapper> _mockMapper;
        private Mock<IRepository> _mockRepository;

        [TestInitialize]
        public void Init()
        {
            _mockRepository = new Mock<IRepository>();
            _wasteService = new WasteService(
                _mockRepository.Object);
        }

        [TestMethod]
        public async Task WasteTypes_Returned_Succesfully()
        {
            // Arrange
            

            // Act
            var wasteTypes = await _wasteService.WasteTypes();

            // Assert
            _mockRepository.Verify(r => r.GetAllWasteTypes(), Times.Once()); // test we called the expected function on the repo
        }
    }
}
