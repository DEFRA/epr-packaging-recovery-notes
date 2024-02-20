using EPRN.Common.Enums;
using EPRN.Portal.Areas.Reprocessor.Controllers;
using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels.PRNS;
using EPRN.Portal.ViewModels.Waste;
using Microsoft.AspNetCore.Mvc;
using Moq;
using static EPRN.Common.Constants.Strings;

namespace EPRN.UnitTests.Portal.Controllers.Areas.Reprocessor
{
    [TestClass]
    public class PRNSControllerTests
    {
        private PRNSController _prnController;
        private Mock<IPRNService> _mockPrnService;
        private WasteCommonViewModel _mockWasteCommonViewModel;

        [TestInitialize]
        public void Init()
        {
            _mockPrnService = new Mock<IPRNService>();
            var factory = new Func<EPRN.Common.Enums.Category, IPRNService>((category) => _mockPrnService.Object);
            _mockWasteCommonViewModel = new WasteCommonViewModel();
            _prnController = new PRNSController(factory, _mockWasteCommonViewModel);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ThrowsException_NullParameterProvided()
        {
            _prnController = new PRNSController(null);
        }

        [TestMethod]
        public async Task Tonnes_ReturnsNotFound_NoIdProvided()
        {
            // arrange
            int? id = null;

            // act
            var result = await _prnController.Tonnes(id);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Tonnes_CallsService_ValidParameter()
        {
            // arrange
            var id = 3;

            // act
            var result = await _prnController.Tonnes(id);

            // assert
            _mockPrnService.Verify(s =>
                s.GetTonnesViewModel(
                    It.Is<int>(p => p == id)),
                Times.Once);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            var viewResult = (ViewResult)result;

            // the view is the same as the action, therefore no name expected
            Assert.IsNull(viewResult.ViewName);
        }

        [TestMethod]
        public async Task Tonnes_ReturnsView_WhenModelStateInvalid()
        {
            // arrange
            var tonnesViewModel = new TonnesViewModel();

            _prnController.ModelState.AddModelError("error", "error message"); // ensure model state is invalid

            // act
            var result = await _prnController.Tonnes(tonnesViewModel);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            var viewResult = result as ViewResult;
            Assert.IsNull(viewResult.ViewName);
        }

        [TestMethod]
        public async Task Tonnes_Redirects_WhenModelStateIsValid()
        {
            // arrange
            var id = 6;
            var tonnes = 5.7;
            var tonnesViewModel = new TonnesViewModel
            {
                Id = id,
                Tonnes = tonnes,
            };

            // act
            var result = await _prnController.Tonnes(tonnesViewModel);

            // assert
            _mockPrnService.Verify(s =>
                s.SaveTonnes(
                    It.Is<TonnesViewModel>(p => p.Id == id && p.Tonnes == tonnes)),
                Times.Once());
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));

            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("SentTo", redirectResult.ActionName);
            Assert.AreEqual("PRNS", redirectResult.ControllerName);
            var routeValues = redirectResult.RouteValues.FirstOrDefault();

            Assert.IsNotNull(routeValues);
            Assert.AreEqual("area", routeValues.Key);
            Assert.AreEqual(string.Empty, routeValues.Value);
        }

        [TestMethod]
        public async Task Confirmation_ReturnsNotFound_WhenNoParametersSupplied()
        {
            // arrange
            int? id = null;

            // act
            var result = await _prnController.Confirmation(id);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Confirmation_ReturnsViewResult_WhenValidParametersSupplied()
        {
            // arrange
            var id = 3;

            // act
            var result = await _prnController.Confirmation(id);

            // assert
            _mockPrnService.Verify(s =>
                s.GetConfirmation(
                    It.Is<int>(p => p == id)),
                Times.Once);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            var viewResult = (ViewResult)result;

            // the view is the same as the action, therefore no name expected
            Assert.IsNull(viewResult.ViewName);
        }

        [TestMethod]
        public async Task CheckYourAnswers_ReturnsNotFound_WhenNoIdSupplied()
        {
            // arrange

            // act
            var result = await _prnController.CheckYourAnswers((int?)null);

            // assert
            _mockPrnService.Verify(s => s.GetCheckYourAnswersViewModel(It.IsAny<int>()), Times.Never);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task CheckYourAnswers_ReturnsViewResult_WhenValidParametersSupplied()
        {
            // arrange
            var id = 402;

            // act
            var result = await _prnController.CheckYourAnswers(id);

            // assert
            _mockPrnService.Verify(s =>
                s.GetCheckYourAnswersViewModel(
                    It.Is<int>(p => p == id)),
                Times.Once);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            var viewResult = (ViewResult)result;
            Assert.IsNotNull(viewResult);
            Assert.IsNull(viewResult.ViewName);
        }

        [TestMethod]
        public async Task CheckYourAnswers_CallsService_WhenValidModelSupplied()
        {
            // arrange
            var model = new CheckYourAnswersViewModel
            {
                Id = 4536
            };

            // act
            await _prnController.CheckYourAnswers(model);

            // assert
            _mockPrnService.Verify(s =>
                s.SaveCheckYourAnswers(
                    It.Is<int>(p => p == model.Id)),
                Times.Once);
        }

        [TestMethod]
        public async Task PRNCancellation_Get_ReturnsNotFound_When_IdIsNull()
        {
            // Arrange
            int? id = null;

            // Act
            var result = await _prnController.PRNCancellation(id);

            // Assert
            _mockPrnService.Verify(s => s.GetCancelViewModel(It.IsAny<int>()), Times.Never);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task PRNCancellation_Get_ReturnsView_When_StatusIsNotCancelled()
        {
            // Arrange
            int id = 123;
            var viewModel = new CancelViewModel();
            viewModel.Status = PrnStatus.Draft;

            _mockPrnService.Setup(service => service.GetCancelViewModel(id)).ReturnsAsync(viewModel);

            // Act
            var result = await _prnController.PRNCancellation(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            _mockPrnService.Verify(s => s.GetCancelViewModel(It.Is<int>(p => p == id)), Times.Once);
        }

        [TestMethod]
        public async Task PRNCancellation_Get_ReturnsViewForCancelled_When_StatusIsCancelled()
        {
            // Arrange
            int id = 123;
            var viewModel = new CancelViewModel();
            viewModel.Status = PrnStatus.Cancelled;

            _mockPrnService.Setup(service =>
                service.GetCancelViewModel(id))
            .ReturnsAsync(viewModel);

            // Act
            var result = await _prnController.PRNCancellation(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            _mockPrnService.Verify(s =>
                s.GetCancelViewModel(
                    It.Is<int>(p => p == id)),
                Times.Once);
        }

        [TestMethod]
        public async Task PRNCancellation_Post_ReturnsView_When_ModelStateIsNotValid()
        {
            // Arrange
            var cancelViewModel = new CancelViewModel();

            _prnController.ModelState.AddModelError("PropertyName", "Error Message");

            // Act
            var result = await _prnController.PRNCancellation(cancelViewModel);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreSame(cancelViewModel, ((ViewResult)result).Model);
        }

        [TestMethod]
        public async Task PRNCancellation_Post_CallsServiceAndReturnsViewForCancelled_When_ModelStateIsValid()
        {
            // Arrange
            var cancelViewModel = new CancelViewModel();
            _mockPrnService.Setup(service => service.CancelPRN(cancelViewModel));

            // Act
            var result = await _prnController.PRNCancellation(cancelViewModel);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            _mockPrnService.Verify(s =>
                s.CancelPRN(
                    It.Is<CancelViewModel>(p => p == cancelViewModel)),
            Times.Once);
        }

        [TestMethod]
        public async Task CancelAcceptedPERN_Get_ValidId_ReturnsView()
        {
            // Arrange
            int id = 1;
            var viewModel = new RequestCancelViewModel { Status = PrnStatus.CancellationRequested };
            _mockPrnService.Setup(x => x.GetRequestCancelViewModel(id)).ReturnsAsync(viewModel);

            // Act
            var result = await _prnController.CancelAcceptedPRN(id) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(viewModel, result.Model);
            Assert.AreEqual(
                Routes.Areas.Actions.PRNS.RequestCancelConfirmed,
                result.ViewName);
        }

        [TestMethod]
        public async Task CancelAcceptedPERN_Get_InvalidId_ReturnsNotFound()
        {
            // Arrange
            int? id = null;

            // Act
            var result = await _prnController.CancelAcceptedPRN(id) as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task CancelAcceptedPERN_Post_ValidModel_ReturnsView()
        {
            // Arrange
            var viewModel = new RequestCancelViewModel();
            _mockPrnService.Setup(x => x.RequestToCancelPRN(viewModel));

            // Act
            var result = await _prnController.CancelAcceptedPRN(viewModel) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(Routes.Areas.Actions.PRNS.RequestCancelConfirmed, result.ViewName);
            Assert.AreEqual(viewModel, result.Model);
            _mockPrnService.Verify(s =>
                s.RequestToCancelPRN(
                    It.Is<RequestCancelViewModel>(p => p == viewModel)),
                Times.Once);
        }

        [TestMethod]
        public async Task CancelAcceptedPERN_Post_InvalidModel_ReturnsViewWithModel()
        {
            // Arrange
            var viewModel = new RequestCancelViewModel();
            _prnController.ModelState.AddModelError("PropertyName", "Error Message");

            // Act
            var result = await _prnController.CancelAcceptedPRN(viewModel) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(viewModel, result.Model);
        }

        #region DeleteDraftPrn

        [TestMethod]
        public async Task DeleteDraftPrn_ReturnsCorrectViewModelAndView()
        {
            // Arrange
            var expectedViewModel = new DeleteDraftPrnViewModel();
            _mockPrnService.Setup(service => service.GetDeleteDraftPrnViewModel(It.IsAny<int>())).ReturnsAsync(expectedViewModel);

            // Act
            var result = await _prnController.DeleteDraftPrn(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedViewModel, result.Model);
            //Assert.AreEqual("DeleteDraft", result.ViewName);
            Assert.IsNull(result.ViewName);
        }

        [TestMethod]
        public async Task DeleteDraftPrn_ReturnsRedirectToActionResult()
        {
            // Arrange
            var viewModel = new DeleteDraftPrnViewModel();

            // Act
            var result = await _prnController.DeleteDraftPrn(viewModel);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }

        [TestMethod]
        public async Task DeleteDraftPrn_CallsDeleteDraftPrn_InPrnService()
        {
            // Arrange
            var viewModel = new DeleteDraftPrnViewModel();

            // Act
            await _prnController.DeleteDraftPrn(viewModel);

            // Assert
            _mockPrnService.Verify(m => m.DeleteDraftPrn(viewModel), Times.Once());
        }

        [TestMethod]
        public async Task DeleteDraftPrn_RedirectsToCorrectView_WithCorrectParameters()
        {
            // Arrange
            var viewModel = new DeleteDraftPrnViewModel { Id = 1 };

            // Act
            var result = await _prnController.DeleteDraftPrn(viewModel) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("ViewDraftPRNS", result.ActionName);
            Assert.AreEqual(1, result.RouteValues["Id"]);
        }

        [TestMethod]
        public async Task DeleteDraftPrn_ReturnsBadRequest_WhenViewModelIsNull()
        {
            // Arrange

            // Act
            var result = await _prnController.DeleteDraftPrn(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        #endregion

        [TestMethod]
        public async Task DraftConfirmation_ReturnsCorrectViewName_WhenJourneyIdValid()
        {
            // Arrange
            var journeyId = 1;

            var expectedViewModel = new DraftConfirmationViewModel
            {
                Id = journeyId,
                DoWithPRN = null
            };

            _mockPrnService.Setup(service => service.GetDraftConfirmationViewModel(It.IsAny<int>())).ReturnsAsync(expectedViewModel);

            // Act
            var result = await _prnController.DraftConfirmation(journeyId) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsNull(result.ViewName);
        }

        [TestMethod]
        public async Task DraftConfirmation_ReturnsNotFound_WhenJourneyIdIsNull()
        {
            // Arrange
            int? id = null;

            // Act
            var result = await _prnController.DraftConfirmation(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            var notFoundResult = result as NotFoundResult;

            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [TestMethod]
        public async Task DraftConfirmation_CallsGetDraftPrnConfirmationModel_WhenJourneyIdIsValid()
        {
            // Arrange
            var journeyId = 1;
            var expectedViewModel = new DraftConfirmationViewModel();
            _mockPrnService.Setup(service => service.GetDraftConfirmationViewModel(It.IsAny<int>())).ReturnsAsync(expectedViewModel);

            // Act
            var result = await _prnController.DraftConfirmation(journeyId) as ViewResult;

            // Assert
            _mockPrnService.Verify(service => service.GetDraftConfirmationViewModel(journeyId), Times.Once());

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedViewModel, result.Model);
        }

        [TestMethod]
        public async Task DraftConfirmation_ReturnsCorrectViewModel_WhenJourneyIdIsValid()
        {
            // Arrange
            var journeyId = 1;

            var expectedViewModel = new DraftConfirmationViewModel
            {
                Id = journeyId,
                ReferenceNumber = "PRN282472GB",
                DoWithPRN = PrnStatus.Created
            };

            _mockPrnService.Setup(service => service.GetDraftConfirmationViewModel(It.IsAny<int>())).ReturnsAsync(expectedViewModel);

            // Act
            var result = await _prnController.DraftConfirmation(journeyId) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(DraftConfirmationViewModel));
            Assert.AreEqual(expectedViewModel, result.Model);
        }

    }
}
