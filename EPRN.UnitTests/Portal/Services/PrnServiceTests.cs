using AutoMapper;
using EPRN.Common.Dtos;
using EPRN.Common.Enums;
using EPRN.Portal.RESTServices;
using EPRN.Portal.RESTServices.Interfaces;
using EPRN.Portal.Services;
using EPRN.Portal.ViewModels.PRNS;
using Moq;

namespace EPRN.UnitTests.Portal.Services
{
    [TestClass]
    public class PrnServiceTests
    {
        private PRNService _prnService;
        private Mock<IMapper> _mockMapper;
        private Mock<IHttpPrnsService> _mockHttpPrnsService;

        [TestInitialize]
        public void Init()
        {
            _mockHttpPrnsService = new Mock<IHttpPrnsService>();
            _mockMapper = new Mock<IMapper>();
            
            _prnService = new PRNService(
                _mockMapper.Object,
                _mockHttpPrnsService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PrnService_ThrowsException_WhenNoMapperSupplied()
        {
            _prnService = new PRNService(
                null,
                _mockHttpPrnsService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PrnService_ThrowsException_WhenNoHttpPrnServiceSupplied()
        {
            _prnService = new PRNService(
                _mockMapper.Object,
                null);
        }

        [TestMethod]
        public async Task GetTonnesViewModel_CallsHttpService_WithCorrectParameters()
        {
            // arrange
            var id = 8;

            // act
            await _prnService.GetTonnesViewModel(id);

            // assert
            _mockHttpPrnsService.Verify(s => 
                s.GetPrnTonnage(
                    It.Is<int>(p => p == id)), 
                Times.Once);
        }

        [TestMethod]
        public async Task SaveTonnes_CallsService_WithCorrectParameters()
        {
            // arrange
            var id = 9;
            var tonnes = 4.89;

            var viewModel = new TonnesViewModel
            {
                Id = id,
                Tonnes = tonnes,
            };

            // act
            await _prnService.SaveTonnes(viewModel);

            // assert
            _mockHttpPrnsService.Verify(s => 
                s.SaveTonnage(
                    It.Is<int>(p => p == id),
                    It.Is<double>(p => p == tonnes)), 
                Times.Once);
        }

        [TestMethod]
        public async Task GetConfirmation_CallService_WithCorrectParameters()
        {
            // arrange
            var id = 3;
            _mockHttpPrnsService.Setup(s => s.GetConfirmation(id)).ReturnsAsync(new ConfirmationDto());

            // act
            await _prnService.GetConfirmation(id);

            // assert
            _mockHttpPrnsService.Verify(s => 
                s.GetConfirmation(
                    It.Is<int>(p => p == id)), 
                Times.Once);
        }

        [TestMethod]
        public async Task GetCancelViewModel_CallsService_Successfully()
        {
            // arrange
            var id = 123;
            var status = PrnStatus.Sent;
            _mockHttpPrnsService.Setup(s => s.GetStatus(id)).ReturnsAsync(status);

            // act
           var viewModel = await _prnService.GetCancelViewModel(id);

            // assert
            _mockHttpPrnsService.Verify(s => 
                s.GetStatus(
                    It.Is<int>(p => p == id)), 
                Times.Once);
            Assert.IsNotNull(viewModel);
            Assert.AreEqual(status, viewModel.Status);
            Assert.AreEqual(id, viewModel.Id);
        }

        [TestMethod]
        public async Task CancelPRN_CallsService_Successfully()
        {
            // arrange
            var id = 432;
            var reason = "fsdgdsfg";
            var viewModel = new CancelViewModel
            {
                Id = id,
                CancelReason = reason,
            };

            // act
            await _prnService.CancelPRN(viewModel);

            // assert
            _mockHttpPrnsService.Verify(s => 
                s.CancelPRN(
                    It.Is<int>(p => p == id),
                    It.Is<string>(p => p == reason)),
                Times.Once);
        }

        [TestMethod]
        public async Task GetRequestCancelViewModel_ShouldReturnViewModel()
        {
            // Arrange
            int id = 1;
            var expectedDto = new StatusAndProducerDto(); // Replace YourDto with the actual DTO type

            _mockHttpPrnsService
                .Setup(x => x.GetStatusAndProducer(It.IsAny<int>()))
                .ReturnsAsync(expectedDto);

            // Act
            var result = await _prnService.GetRequestCancelViewModel(id);

            // Assert
            _mockHttpPrnsService.Verify(s => 
                s.GetStatusAndProducer(
                    It.Is<int>(p => p == id)), 
                Times.Once);
            _mockMapper.Verify(m => 
                m.Map<RequestCancelViewModel>(
                    It.Is<StatusAndProducerDto>(p => p == expectedDto)), 
                Times.Once);
        }
    }
}
