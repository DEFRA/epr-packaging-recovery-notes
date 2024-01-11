using AutoMapper;
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
            _mockMapper = new Mock<IMapper>();
            _mockHttpPrnsService = new Mock<IHttpPrnsService>();
            _prnService = new PRNService(
                _mockMapper.Object,
                _mockHttpPrnsService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PrnService_ThrowsException_WhenNoMapperSupplied()
        {
            _prnService = new PRNService(null, _mockHttpPrnsService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PrnService_ThrowsException_WhenNoHttpPrnServiceSupplied()
        {
            _prnService = new PRNService(_mockMapper.Object, null);
        }

        [TestMethod]
        public async Task GetTonnesViewModel_CallsHttpService_WithCorrectParameters()
        {
            // arrange
            var id = 8;

            // act
            await _prnService.GetTonnesViewModel(id);

            // assert
            _mockHttpPrnsService.Verify(s => s.GetPrnTonnage(It.Is<int>(p => p == id)), Times.Once);
        }

        [TestMethod]
        public async Task SaveTonnes_CallsService_WithCorrectParameters()
        {
            // arrange
            var id = 9;
            var tonnes = 4.89;

            var viewModel = new TonnesViewModel
            {
                JourneyId = id,
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

            // act
            await _prnService.GetConfirmation(id);

            // assert
            _mockHttpPrnsService.Verify(s => 
                s.GetConfirmation(
                    It.Is<int>(p => p == id)), 
                Times.Once);
        }
    }
}
