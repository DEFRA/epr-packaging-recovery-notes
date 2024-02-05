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
        public async Task ViewPRN_Action_With_Valid_Id_Should_Return_ViewResult_With_ViewModel()
        {
            // Arrange
            var idValue = "PRN1";
            var expectedViewModel = new ViewPRNViewModel(); // Replace with the actual ViewModel type

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
