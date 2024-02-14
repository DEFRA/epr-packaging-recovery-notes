using EPRN.Common.Dtos;
using EPRN.Common.Enums;
using EPRN.PRNS.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

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

        [TestMethod]
        public async Task GetStatus_ReturnsPopulatedOk()
        {
            // arrange
            var id = 2435;
            var status = PrnStatus.Sent;
            _mockPrnService.Setup(s => s.GetStatus(id)).ReturnsAsync(PrnStatus.Sent);

            // act
            var result = await _prnController.GetStatus(id);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof (OkObjectResult));
            var okObjectResult = (OkObjectResult)result;

            Assert.AreEqual(status, okObjectResult.Value);
        }

        [TestMethod]
        public async Task CancelPrn_CallsService()
        {
            // arrange
            var id = 234;
            var reason = "sdfsfdsdf";

            // act
            await _prnController.CancelPrn(id, reason);

            // assert
            _mockPrnService.Verify(s => 
                s.CancelPrn(
                    It.Is<int>(p => p == id),
                    It.Is<string>(p => p == reason)),
                Times.Once);
        }

        [TestMethod]
        public async Task GetStatusAndProducer()
        {
            // arrange
            var id = 324;
            var statusAndProducerDto = new StatusAndProducerDto
            {
                Status = PrnStatus.Accepted,
                Producer = "A producer"
            };
            _mockPrnService.Setup(s => s.GetStatusWithProducerName(id)).ReturnsAsync(statusAndProducerDto);

            // act
            var result = await _prnController.GetStatusAndProducer(id);

            // assert
            _mockPrnService.Verify(s => 
                s.GetStatusWithProducerName(
                    It.Is<int>(p => p == id)),
                Times.Once);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okObjectResult = (OkObjectResult)result;

            Assert.AreEqual(statusAndProducerDto, okObjectResult.Value);
        }

        [TestMethod]
        public async Task RequestCancelPrn_ShouldReturnOkResult()
        {
            // Arrange
            int id = 1;
            string reason = "Cancellation Reason";

            // Act
            var result = await _prnController.RequestCancelPrn(id, reason);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkResult));

            // Ensure that the service method was called with the correct arguments
            _mockPrnService.Verify(x => 
                x.RequestCancelPrn(
                    It.Is<int>(prnId => prnId == id),
                    It.Is<string>(cancelReason => cancelReason == reason)),
                Times.Once);
        }

        [TestMethod]
        public async Task GetPrnDetails_WhenDtoIsNotNull_ShouldReturnOkResult()
        {
            // Arrange
            var reference = "YourReferenceValue";
            var expectedDto = new PRNDetailsDto();

            _mockPrnService.Setup(service => service.GetPrnDetails(reference))
                .ReturnsAsync(expectedDto);

            // Act
            var result = await _prnController.GetPrnDetails(reference);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            var okResult = (OkObjectResult)result;
            Assert.AreEqual(expectedDto, okResult.Value);
        }

        [TestMethod]
        public async Task GetPrnDetails_WhenDtoIsNull_ShouldReturnNotFoundResult()
        {
            // Arrange
            var reference = "YourReferenceValue";

            _mockPrnService.Setup(service => service.GetPrnDetails(reference))
                .ReturnsAsync((PRNDetailsDto)null);

            // Act
            var result = await _prnController.GetPrnDetails(reference);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetDecemberWaste_Returns_OK()
        {
            //Arrange
            var id = 3;

            //Act
            var result = await _prnController.GetDecemberWaste(id);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            _mockPrnService.Verify(s => s.GetDecemberWaste(It.Is<int>(p => p == id)));
        }

        [TestMethod]
        public async Task SaveDecemberWaste_CallService_WithValidParameters()
        {
            // arrange
            var id = 2;

            // act
            var result = await _prnController.SaveDecemberWaste(id, true);

            // assert
            _mockPrnService.Verify(s =>
                s.SaveDecemberWaste(
                    It.IsAny<int>(), true),
                Times.Once);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task GetPrnDetailsUsingId_WhenDtoIsNotNull_ShouldReturnOkResult()
        {
            // Arrange
            var reference = 1;
            var expectedDto = new PRNDetailsDto();

            _mockPrnService.Setup(service => service.GetPrnDetails(reference))
                .ReturnsAsync(expectedDto);

            // Act
            var result = await _prnController.GetPrnDetails(reference);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            var okResult = (OkObjectResult)result;
            Assert.AreEqual(expectedDto, okResult.Value);
        }

        [TestMethod]
        public async Task GetPrnDetailsUsingId_WhenDtoIsNull_ShouldReturnNotFoundResult()
        {
            // Arrange
            var reference = 0;

            _mockPrnService.Setup(service => service.GetPrnDetails(reference))
                .ReturnsAsync((PRNDetailsDto)null);

            // Act
            var result = await _prnController.GetPrnDetails(reference);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}
