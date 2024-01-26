using EPRN.Portal.Areas.Exporter.Controllers;
using EPRN.PRNS.API.Controllers;
using EPRN.PRNS.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPRN.UnitTests.API.PRNS.Controllers
{
    [TestClass]
    public class PRNControllerTests
    {
        private EPRN.PRNS.API.Controllers.PRNController _prnController;
        private Mock<IPrnService> _mockPrnService;

        [TestInitialize]
        public void Init()
        {
            _mockPrnService = new Mock<IPrnService>();
            _prnController = new EPRN.PRNS.API.Controllers.PRNController(_mockPrnService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "prnService")]
        public void ConstructingController_ThrowsException_WhenNullServicePassedIn()
        {
            // Arrange
            // Act
            new EPRN.PRNS.API.Controllers.PRNController(null);

            // Assert - covered by expected exception attribute
        }

        [TestMethod]
        public async Task GetTonnage_CallsService_WhenIdProvided()
        {
            // Arrange
            var id = 5;

            // Act
            var result = await _prnController.GetTonnage(id);

            // Assert
            Assert.IsNotNull(result);
            _mockPrnService.Verify(s => s.GetTonnage(It.Is<int>(p => p == id)), Times.Once());
        }

        [TestMethod]
        public async Task GetTonnage_ReturnsBadRequest_WhenNoIdSupplied()
        {
            // Arrange

            // Act
            var result = await _prnController.GetTonnage(null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task SaveTonnage_CallsService_WhenParametersProvided()
        {
            // Arrange
            var id = 7;
            var tonnage = 4.5;

            // Act
            var result = await _prnController.SaveTonnage(id, tonnage);

            // Assert
            Assert.IsNotNull(result);
            _mockPrnService.Verify(s => s.SaveTonnage(It.Is<int>(p => p == id), It.Is<double>(p => p == tonnage)), Times.Once());
        }

        [TestMethod]
        public async Task SaveTonnage_ReturnsBadRequest_WhenNoIdSupplied()
        {
            // Arrange

            // Act
            var result = await _prnController.SaveTonnage(null, 1);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task SaveTonnage_ReturnsBadRequest_WhenNoTonnageSupplied()
        {
            // Arrange

            // Act
            var result = await _prnController.SaveTonnage(1, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task GetConfirmation_ReturnsBadRequest_WhenNoIdSupplied()
        {
            // Arragne

            // Act
            var result = await _prnController.GetConfirmation(null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task GetConfirmation_Succeeds_WhenParametersProvided()
        {
            // arrange
            var id = 4;

            // act
            await _prnController.GetConfirmation(id);

            // assert
            _mockPrnService.Verify(s => 
                s.GetConfirmation(
                    It.Is<int>(p => p == id)), 
                Times.Once);
        }

        [TestMethod]
        public async Task CheckYourAnswers_ReturnsBadRequest_WithNullParameter()
        {
            // arrange

            // act
            var result = await _prnController.CheckYourAnswers(null);

            // assert
            _mockPrnService.Verify(s =>
                s.GetCheckYourAnswers(It.IsAny<int>()),
                Times.Never);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task CheckYourAnswers_CallsService_WithValidParameter()
        {
            // arrange
            var id = 2;

            // act
            var result = await _prnController.CheckYourAnswers(id);

            // assert
            _mockPrnService.Verify(s =>
                s.GetCheckYourAnswers(It.Is<int>(p => p == id)),
                Times.Once);
        }

        [TestMethod]
        public async Task SaveCheckYourAnswersState_ReturnsBadRequest_WhenNullParameterSupplied()
        {
            // arrange

            // act
            var result = await _prnController.SaveCheckYourAnswersState(null);

            // assert
            _mockPrnService.Verify(s => 
                s.SaveCheckYourAnswers(
                    It.IsAny<int>()), 
                Times.Never);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof (BadRequestObjectResult));
        }

        [TestMethod]
        public async Task SaveCheckYourAnswersState_CallService_WithValidParameters()
        {
            // arrange
            var id = 423;

            // act
            var result = await _prnController.SaveCheckYourAnswersState(id);

            // assert
            _mockPrnService.Verify(s =>
                s.SaveCheckYourAnswers(
                    It.IsAny<int>()),
                Times.Once);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }
    }
}
