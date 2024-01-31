﻿using EPRN.Common.Enums;
using EPRN.Portal.Areas.Exporter.Controllers;
using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels.PRNS;
using Microsoft.AspNetCore.Mvc;
using Moq;
using static EPRN.Common.Constants.Strings;

namespace EPRN.UnitTests.Portal.Controllers.Areas.Exporter
{
    [TestClass]
    public class PRNSControllerTests
    {
        private PRNSController _prnController;
        private Mock<IPRNService> _mockPrnService;

        [TestInitialize]
        public void Init()
        {
            _mockPrnService = new Mock<IPRNService>();
            _prnController = new PRNSController(_mockPrnService.Object);
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
            Assert.AreEqual(Category.Exporter, routeValues.Value);
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
        public async Task PRNCancellation_CallsService_WhenValidParametersSupplied()
        {
            // arrange
            var id = 45;

            // act
            await _prnController.PRNCancellation(id);

            // assert
            _mockPrnService.Verify(s => s.GetCancelViewModel(It.Is<int>(p => p == id)), Times.Once);
        }

        [TestMethod]
        public async Task PRNCancellation_ReturnsNotFound_WhenNoIdSupplied()
        {
            // arrange

            // act
            var result = await _prnController.PRNCancellation((int?)null);

            // assert
            _mockPrnService.Verify(s => s.GetCancelViewModel(It.IsAny<int>()), Times.Never);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task PRNCancellation_CallServiceAndRedirects_WhenValidModelSupplied()
        {
            // arrange
            var id = 34;
            var reason = "whatever";
            var viewModel = new CancelViewModel
            {
                Id = id,
                CancelReason = reason
            };

            // act
            var result = await _prnController.PRNCancellation(viewModel);

            // assert
            _mockPrnService.Verify(s => s.CancelPRN(It.Is<CancelViewModel>(p =>
                p.Id == id && p.CancelReason == reason)),
                Times.Once
            );
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));

            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual(Routes.Areas.Actions.PRNS.Cancelled, redirectResult.ActionName);
            Assert.AreEqual(Routes.Areas.Controllers.Exporter.PRNS, redirectResult.ControllerName);
            var routeValues = redirectResult.RouteValues.FirstOrDefault(r => r.Key == "area");

            Assert.IsNotNull(routeValues);
            Assert.AreEqual(Routes.Areas.Exporter, routeValues.Value);

            routeValues = redirectResult.RouteValues.FirstOrDefault(r => r.Key == "id");
            Assert.IsNotNull(routeValues);
            Assert.AreEqual(id.ToString(), routeValues.Value.ToString());
        }

        [TestMethod]
        public async Task PRNCancellation_ReturnsView_WhenModelStateInvalid()
        {
            // arrange
            _prnController.ModelState.AddModelError("Err", "Err");
            var id = 34;
            var reason = "whatever";
            var viewModel = new CancelViewModel
            {
                Id = id,
                CancelReason = reason
            };

            // act
            var result = await _prnController.PRNCancellation(viewModel);

            // assert
            _mockPrnService.Verify(s => s.CancelPRN(It.IsAny<CancelViewModel>()), Times.Never);
            _mockPrnService.Verify(s => s.GetCancelViewModel(It.Is<int>(p => p == id)), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}
