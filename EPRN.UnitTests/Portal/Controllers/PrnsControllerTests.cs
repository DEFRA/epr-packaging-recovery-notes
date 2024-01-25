using EPRN.Portal.Controllers;
using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels.PRNS;
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

        [TestMethod]
        public async Task PrnSavedAsDraftConfirmation_ReturnsNotFound_WhenJourneyIdIsNull()
        {
            // Arrange

            // Act
            var result = await _prnsController.PrnSavedAsDraftConfirmation(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            var notFoundResult = result as NotFoundResult;

            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [TestMethod]
        public async Task PrnSavedAsDraftConfirmation_ReturnsBadRequest_WhenJourneyIdZero()
        {
            // Arrange

            // Act
            var result = await _prnsController.PrnSavedAsDraftConfirmation(0);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            var badRequestResult = result as BadRequestResult;

            Assert.AreEqual(400, badRequestResult.StatusCode);
        }

        [TestMethod]
        public async Task PrnSavedAsDraftConfirmation_CallsGetDraftPrnConfirmationModel_WhenJourneyIdIsValid()
        {
            // Arrange
            var journeyId = 1;
            var expectedViewModel = new PrnSavedAsDraftViewModel();
            _mockPrnService.Setup(service => service.GetDraftPrnConfirmationModel(It.IsAny<int>())).ReturnsAsync(expectedViewModel);

            // Act
            var result = await _prnsController.PrnSavedAsDraftConfirmation(journeyId) as ViewResult;

            // Assert
            _mockPrnService.Verify(service => service.GetDraftPrnConfirmationModel(journeyId), Times.Once());

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedViewModel, result.Model);
        }

        [TestMethod]
        public async Task PrnSavedAsDraftConfirmation_ReturnsCorrectViewModel_WhenJourneyIdIsValid()
        {
            // Arrange
            var journeyId = 1;

            var expectedViewModel = new PrnSavedAsDraftViewModel
            {
                Id = journeyId,
                PrnNumber = "PRN282472GB"
            };

            _mockPrnService.Setup(service => service.GetDraftPrnConfirmationModel(It.IsAny<int>())).ReturnsAsync(expectedViewModel);

            // Act
            var result = await _prnsController.PrnSavedAsDraftConfirmation(journeyId) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(PrnSavedAsDraftViewModel));
            Assert.AreEqual(expectedViewModel, result.Model);
        }

        [TestMethod]
        public async Task PrnSavedAsDraftConfirmation_ReturnsCorrectViewName_WhenJourneyIdValid()
        {
            // Arrange
            var journeyId = 1;

            var expectedViewModel = new PrnSavedAsDraftViewModel
            {
                Id = journeyId,
                PrnNumber = "PRN282472GB"
            };

            _mockPrnService.Setup(service => service.GetDraftPrnConfirmationModel(It.IsAny<int>())).ReturnsAsync(expectedViewModel);

            // Act
            var result = await _prnsController.PrnSavedAsDraftConfirmation(journeyId) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsNull(result.ViewName);
        }

        [TestMethod]
        public async Task ViewSentPrns_ReturnsViewResult()
        {
            // Arrange

            // Act
            var result = await _prnsController.ViewSentPrns();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));

        }

        [TestMethod]
        public async Task ViewSentPrns_CallsGetViewSentPrnsViewModel()
        {
            // Arrange

            // Act
            var result = await _prnsController.ViewSentPrns();

            // Assert
            _mockPrnService.Verify(service => service.GetViewSentPrnsViewModel(1), Times.Once());
        }

        [TestMethod]
        public async Task ViewSentPrns_ReturnsCorrectViewModel()
        {
            // Arrange
            var expectedViewModel = new ViewSentPrnsViewModel();
            _mockPrnService.Setup(service => service.GetViewSentPrnsViewModel(It.IsAny<int>())).ReturnsAsync(expectedViewModel);

            // Act
            var result = await _prnsController.ViewSentPrns();

            // Assert
            Assert.AreEqual(expectedViewModel, (result as ViewResult)?.Model);
        }
    }
}
