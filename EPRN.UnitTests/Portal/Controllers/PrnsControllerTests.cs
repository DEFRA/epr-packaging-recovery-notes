using EPRN.Common.Dtos;
using EPRN.Portal.Controllers;
using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels.PRNS;
using EPRN.PRNS.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EPRN.UnitTests.Portal.Controllers
{
    [TestClass]
    public class PrnsControllerTests
    {
        private Mock<IPRNService> _mockPrnService;
        private PrnsController _prnsController;

        [TestInitialize]
        public void Init()
        {
            _mockPrnService = new Mock<IPRNService>();
            _prnsController = new PrnsController(_mockPrnService.Object);
        }

        #region ViewSentPrns

        [TestMethod]
        public async Task ViewSentPrns_ReturnsViewResultWithCorrectModel()
        {
            // Arrange
            var request = new GetSentPrnsViewModel();
            _mockPrnService.Setup(service => service.GetViewSentPrnsViewModel(request)).ReturnsAsync(new ViewSentPrnsViewModel());

            // Act
            var result = await _prnsController.ViewSentPrns(request) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            _mockPrnService.Verify(service => service.GetViewSentPrnsViewModel(request), Times.Once());
        }

        [TestMethod]
        public async Task ViewSentPrns_ReturnsCorrectView()
        {
            // Arrange
            var request = new GetSentPrnsViewModel();
            _mockPrnService.Setup(service => service.GetViewSentPrnsViewModel(request)).ReturnsAsync(new ViewSentPrnsViewModel());

            // Act
            var result = await _prnsController.ViewSentPrns(request) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsNotNull(result, result.ViewName);
            _mockPrnService.Verify(service => service.GetViewSentPrnsViewModel(request), Times.Once());
        }

        [TestMethod]
        public async Task ViewSentPrns_PassesCorrectModelToView()
        {
            // Arrange
            var expectedModel = new ViewSentPrnsViewModel();
            _mockPrnService.Setup(service => service.GetViewSentPrnsViewModel(It.IsAny<GetSentPrnsViewModel>())).ReturnsAsync(expectedModel);

            // Act
            var result = await _prnsController.ViewSentPrns(new GetSentPrnsViewModel()) as ViewResult;

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual(expectedModel, result.Model);
        }

        #endregion

        [TestMethod]
        public async Task ViewPRN_Action_With_Valid_Id_Should_Return_ViewResult_With_ViewModel()
        {
            // Arrange
            var idValue = "PRN1";
            var expectedViewModel = new ViewPRNViewModel();

            _mockPrnService.Setup(service => service.GetViewPrnViewModel(idValue))
                           .ReturnsAsync(expectedViewModel);

            // Act
            var result = await _prnsController.ViewPRN(idValue);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            var viewResult = result as ViewResult;
            Assert.AreEqual(expectedViewModel, viewResult.Model);
        }

        [TestMethod]
        public async Task ViewPRN_Action_With_Null_Id_Should_Return_NotFoundResult()
        {
            // Arrange
            var id = default(string);

            // Act
            var result = await _prnsController.ViewPRN(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}
