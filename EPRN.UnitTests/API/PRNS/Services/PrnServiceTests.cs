using AutoMapper;
using EPRN.Common.Dtos;
using EPRN.Common.Enums;
using EPRN.PRNS.API.Repositories.Interfaces;
using EPRN.PRNS.API.Services;
using Moq;

namespace EPRN.UnitTests.API.PRNS.Services
{
    [TestClass]
    public class PrnServiceTests
    {
        private PrnService _prnService;
        private Mock<IMapper> _mockMapper;
        private Mock<IRepository> _mockRepository;

        [TestInitialize]
        public void Init()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepository = new Mock<IRepository>();

            _prnService = new PrnService(
                _mockMapper.Object,
                _mockRepository.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructService_ThrowsException_WhenMapperNotSupplied()
        {
            new PrnService(null, _mockRepository.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructService_ThrowsException_WhenRepositoryNotSupplied()
        {
            new PrnService(_mockMapper.Object, null);
        }

        [TestMethod]
        public async Task CreatePrnRecord_CallsServiceMethod()
        {
            // Arrange
            var materialId = 3;
            var category = Category.Exporter;

            // Act
            await _prnService.CreatePrnRecord(materialId, category);

            // Assert
            _mockRepository.Verify(s => 
                s.CreatePrnRecord(
                    It.Is<int>(p => p == materialId), 
                    It.Is<Category>(p => p == category),
                    It.IsAny<string>()), 
                Times.Once);
        }

        [TestMethod]
        public async Task GetTonnage_CallsServiceMethod()
        {
            // Arrange
            var id = 6;

            // Act
            await _prnService.GetTonnage(id);

            // Assert
            _mockRepository.Verify(s => 
                s.GetTonnage(
                    It.Is<int>(p => p == id)), 
                Times.Once);
        }

        [TestMethod]
        public async Task SaveTonnage_CallsServiceMethod()
        {
            // Arrange
            var id = 4;
            var tonnage = 6;

            // Act
            await _prnService.SaveTonnage(id, tonnage);

            // Assert
            _mockRepository.Verify(s => 
                s.UpdateTonnage(
                    It.Is<int>(p => p == id),
                    It.Is<double>(p => p == tonnage)), 
                Times.Once);
        }

        [TestMethod]
        public async Task GetCheckYourAnswers_CallsServiceMethod()
        {
            // Arrange
            var id = 6;

            // Act
            await _prnService.GetCheckYourAnswers(id);

            // Assert
            _mockRepository.Verify(s =>
                s.GetCheckYourAnswersData(
                    It.Is<int>(p => p == id)),
                Times.Once);
        }

        [TestMethod]
        public async Task SaveCheckYourAnswers_CallsService_WithComplete()
        {
            // arrange
            var id = 342;
            var reason = "update";

            // act
            await _prnService.SaveCheckYourAnswers(id, reason);

            // assert
            _mockRepository.Verify(s =>
                s.UpdatePrnStatus(
                    It.Is<int>(p => p == id),
                    It.Is<PrnStatus>(p => p == PrnStatus.CheckYourAnswersComplete),
                    It.Is<string>(p => p == reason)),
                Times.Once);
        }

        [TestMethod]
        public async Task GetStatus_CallService_WithParameters()
        {
            // arrange
            var id = 234;

            // act
            await _prnService.GetStatus(id);

            // assert
            _mockRepository.Verify(s => 
                s.GetStatus(
                    It.Is<int>(p => p == id)), 
                Times.Once);
        }

        [TestMethod]
        public async Task CancelPrn_UpdatesStatus_WhenNotAccepted()
        {
            // arrange
            var id = 234;
            var reason = "sdfsfsdf";
            _mockRepository.Setup(s => s.GetStatus(id)).ReturnsAsync(PrnStatus.Sent);

            // act
            await _prnService.CancelPrn(id, reason);

            // assert
            _mockRepository.Verify(s =>
                s.UpdatePrnStatus(
                    It.Is<int>(p => p == id),
                    It.Is<PrnStatus>(p => p == PrnStatus.Cancelled),
                    It.Is<string>(p => p == reason)),
                Times.Once);
        }

        [TestMethod]
        public async Task CancelPrn_DoesNotUpdatesStatus_WhenAccepted()
        {
            // arrange
            var id = 234;
            var reason = "sdfsfsdf";
            _mockRepository.Setup(s => s.GetStatus(id)).ReturnsAsync(PrnStatus.Accepted);

            // act
            await _prnService.CancelPrn(id, reason);

            // assert
            _mockRepository.Verify(s =>
                s.UpdatePrnStatus(
                    It.IsAny<int>(),
                    It.IsAny<PrnStatus>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [TestMethod]
        public async Task GetStatusWithProducerName_CallService()
        {
            // arrange
            var id = 45;

            // act
            await _prnService.GetStatusWithProducerName(id);

            // assert
            _mockRepository.Verify(s =>
                s.GetStatusAndRecipient(
                    It.Is<int>(p => p == id)), 
                Times.Once);
        }

        [TestMethod]
        public async Task RequestCancelPrn_WithAcceptedPrn_ShouldUpdateStatus()
        {
            // Arrange
            int id = 1;
            string reason = "Cancellation Reason";

            _mockRepository
                .Setup(x => x.GetStatus(It.IsAny<int>()))
                .ReturnsAsync(PrnStatus.Accepted);

            // Act
            await _prnService.RequestCancelPrn(id, reason);

            // Assert
            _mockRepository.Verify(x => 
                x.UpdatePrnStatus(
                    It.Is<int>(prnId => prnId == id),
                    It.Is<PrnStatus>(status => status == PrnStatus.CancellationRequested),
                    It.Is<string>(cancelReason => cancelReason == reason)),
                Times.Once);
        }

        [TestMethod]
        public async Task RequestCancelPrn_WithNotAcceptedPrn_ShouldNotUpdateStatus()
        {
            // Arrange
            int id = 1;
            string reason = "Cancellation Reason";

            _mockRepository
                .Setup(x => x.GetStatus(It.IsAny<int>()))
                .ReturnsAsync(PrnStatus.Sent);

            // Act
            await _prnService.RequestCancelPrn(id, reason);

            // Assert
            _mockRepository.Verify(x => 
                x.UpdatePrnStatus(
                    It.IsAny<int>(),
                    It.IsAny<PrnStatus>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [TestMethod]
        public async Task GetPrnDetails_WhenRepositoryReturnsDto_ShouldReturnDto()
        {
            // Arrange
            var reference = "YourReferenceValue";
            var expectedDto = new PRNDetailsDto();

            _mockRepository.Setup(repository => repository.GetDetails(reference))
                .ReturnsAsync(expectedDto);

            // Act
            var result = await _prnService.GetPrnDetails(reference);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedDto, result);
        }

        [TestMethod]
        public async Task GetPrnDetails_WhenRepositoryReturnsNull_ShouldReturnNull()
        {
            // Arrange
            var reference = "YourReferenceValue";

            _mockRepository.Setup(repository => repository.GetDetails(reference))
                .ReturnsAsync((PRNDetailsDto)null);

            // Act
            var result = await _prnService.GetPrnDetails(reference);

            // Assert
            Assert.IsNull(result);
        }
    }
}
