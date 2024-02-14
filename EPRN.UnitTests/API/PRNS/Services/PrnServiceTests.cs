using AutoMapper;
using EPRN.Common.Dtos;
using EPRN.Common.Enums;
using EPRN.PRNS.API.Configuration;
using EPRN.PRNS.API.Repositories.Interfaces;
using EPRN.PRNS.API.Services;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
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
            var config = new Mock<IOptions<AppConfigSettings>>();

            _prnService = new PrnService(
                config.Object,
                _mockMapper.Object,
                _mockRepository.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructService_ThrowsException_WhenMapperNotSupplied()
        {
            new PrnService(
                null, 
                null, 
                _mockRepository.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructService_ThrowsException_WhenRepositoryNotSupplied()
        {
            new PrnService(
                null, 
                _mockMapper.Object, 
                null);
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
            
            // act
            await _prnService.SaveCheckYourAnswers(id);

            // assert
            _mockRepository.Verify(s =>
                s.UpdatePrnStatus(
                    It.IsAny<int>(),
                    It.IsAny<PrnStatus>(),
                    It.IsAny<string>()),
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

        [TestMethod]
        public async Task GetDecemberWaste_CurrentMonthIsJanuary_ReturnsDecemberWasteDto()
        {
            // Arrange
            var config = new Mock<IOptions<AppConfigSettings>>();
            config.SetupGet(x => x.Value).Returns(new AppConfigSettings
            {
                CurrentMonthOverride = 1 // Set current month override to January
            });
            _prnService = new PrnService(
                config.Object,
                _mockMapper.Object,
                _mockRepository.Object);

            var journeyId = 123;
            var decemberWasteDto = new DecemberWasteDto { Id = journeyId, IsWithinMonth = true };
            _mockRepository.Setup(repo => repo.GetDecemberWaste(journeyId)).ReturnsAsync(decemberWasteDto);

            // Act
            var result = await _prnService.GetDecemberWaste(journeyId);

            // Assert
            Assert.IsNotNull(result);
            _mockRepository.Verify(r =>
                r.GetDecemberWaste(
                    It.Is<int>(p => p == journeyId)),
                Times.Once);
        }

        [TestMethod]
        public async Task GetDecemberWaste_CurrentMonthIsDecember_ReturnsDecemberWasteDto()
        {
            // Arrange
            var config = new Mock<IOptions<AppConfigSettings>>();
            config.SetupGet(x => x.Value).Returns(new AppConfigSettings
            {
                CurrentMonthOverride = 12 // Set current month override to December
            });
            _prnService = new PrnService(
                config.Object,
                _mockMapper.Object,
                _mockRepository.Object);

            var journeyId = 123;
            var decemberWasteDto = new DecemberWasteDto { Id = journeyId, IsWithinMonth = true };
            _mockRepository.Setup(repo => repo.GetDecemberWaste(journeyId)).ReturnsAsync(decemberWasteDto);
            
            // Act
            var result = await _prnService.GetDecemberWaste(journeyId);

            // Assert
            Assert.IsNotNull(result);
            _mockRepository.Verify(r => 
                r.GetDecemberWaste(
                    It.Is<int>(p => p == journeyId)), 
                Times.Once);
        }

        [TestMethod]
        public async Task GetDecemberWaste_CurrentMonthIsNotDecember_ReturnsDecemberWasteDtoWithFalseIsWithinMonth()
        {
            // Arrange
            var journeyId = 123;
            var config = new Mock<IOptions<AppConfigSettings>>();
            config.SetupGet(x => x.Value).Returns(new AppConfigSettings
            {
                CurrentMonthOverride = 5 // Set current month override to May
            });
            _prnService = new PrnService(
                config.Object,
                _mockMapper.Object,
                _mockRepository.Object);

            // Act
            var result = await _prnService.GetDecemberWaste(journeyId);

            // Assert
            Assert.IsFalse(result.IsWithinMonth);
            Assert.AreEqual(journeyId, result.Id);
            _mockRepository.Verify(r =>
                r.GetDecemberWaste(
                    It.IsAny<int>()),
                Times.Never);
        }

        [TestMethod]
        public async Task GetDecemberWaste_ConfigNotDefined_ReturnsDecemberWasteDtoWithFalseIsWithinMonth()
        {
            // Arrange
            var journeyId = 123;
            var config = new Mock<IOptions<AppConfigSettings>>();
            config.SetupGet(x => x.Value).Returns(new AppConfigSettings
            {
                CurrentMonthOverride = null // current override month not set
            });
            _prnService = new PrnService(
                config.Object,
                _mockMapper.Object,
                _mockRepository.Object);

            // Act
            var result = await _prnService.GetDecemberWaste(journeyId);

            // Assert
            Assert.IsFalse(result.IsWithinMonth);
            Assert.AreEqual(journeyId, result.Id);
            _mockRepository.Verify(r =>
                r.GetDecemberWaste(
                    It.IsAny<int>()),
                Times.Never);
        }

        [TestMethod]
        public async Task GetPrnDetailsUsingId_WhenRepositoryReturnsDto_ShouldReturnDto()
        {
            // Arrange
            var reference = 1;
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
        public async Task SaveSaveSentTo_CallsServiceMethod()
        {
            // Arrange
            var id = 4;
            string sentTo = "Draft";

            // Act
            await _prnService.SaveSentTo(id, sentTo);

            // Assert
            _mockRepository.Verify(s =>
                s.SaveSentTo(
                    It.Is<int>(p => p == id),
                    It.Is<string>(p => p == sentTo)),
                Times.Once);
        }

        [TestMethod]
        public async Task SaveDecemberWaste_CallsServiceMethod()
        {
            // Arrange
            var id = 4;
            var waste = true;

            // Act
            await _prnService.SaveDecemberWaste(id, waste);

            // Assert
            _mockRepository.Verify(s =>
                s.SaveDecemberWaste(
                    It.Is<int>(p => p == id),
                    It.Is<bool>(p => p == waste)),
                Times.Once);
        }

        [TestMethod]
        public async Task SaveDraftPrn_CallsServiceMethod()
        {
            // Arrange
            var id = 4;
            var reason = "update";

            // Act
            await _prnService.SaveDraftPrn(id, reason);

            // Assert
            _mockRepository.Verify(s =>
                s.UpdatePrnStatus(
                    It.IsAny<int>(),
                    It.IsAny<PrnStatus>(),
                    It.IsAny<string>()),
                Times.Once);
        }
    }
}
