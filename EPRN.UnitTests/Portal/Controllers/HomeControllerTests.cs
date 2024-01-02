using EPRN.Portal.Controllers;
using EPRN.Portal.Helpers.Interfaces;
using EPRN.Portal.Services.Interfaces;
using Moq;

namespace EPRN.UnitTests.Portal.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        private HomeController _homeController;
        private Mock<IHomeService> _mockHomeService;
        private Mock<IHomeServiceFactory> _mockHomeServiceFactory;


        [TestInitialize]
        public void Init()
        {
            _mockHomeService = new Mock<IHomeService>();
            _mockHomeServiceFactory = new Mock<IHomeServiceFactory>();
            _mockHomeServiceFactory.Setup(s => s.CreateHomeService()).Returns(_mockHomeService.Object);
            _homeController = new HomeController(_mockHomeServiceFactory.Object);
        }


        [TestMethod]
        public async Task Index_Calls_Correct_Service_Method_Once()
        {
            // Arrange

            // Act
            var result = await _homeController.Index();

            // Assert
            _mockHomeService.Verify(s => s.GetHomePage(), Times.Once);
        }

    }
}
