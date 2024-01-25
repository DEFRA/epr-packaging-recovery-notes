using EPRN.Portal.Helpers.Filters;
using EPRN.Portal.Services.Interfaces;
using Moq;

namespace EPRN.UnitTests.Portal.Helpers
{
    [TestClass]
    public class WasteTypeActionFilterTests
    {
        private Mock<IWasteService> _mockWasteService;
        private WasteTypeActionFilter _wasteTypeActionFilter;

        [TestInitialize]
        public void Init()
        {
            _mockWasteService = new Mock<IWasteService>();
            _wasteTypeActionFilter = new WasteTypeActionFilter(_mockWasteService.Object);
        }

        [TestMethod]
        public void Constructor_ThrowsException_WhenNullObjectPassedIn()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(() => new WasteTypeActionFilter(null));
            
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(exception, typeof(ArgumentNullException));
            Assert.AreEqual("Value cannot be null. (Parameter 'wasteService')", exception.Message);
        }

        sdgfsdf
    }
}
