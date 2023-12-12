using AutoMapper;
using EPRN.Common.Enums;
using EPRN.Waste.API.Configuration;
using EPRN.Waste.API.Models;
using EPRN.Waste.API.Repositories.Interfaces;
using EPRN.Waste.API.Services;
using EPRN.Waste.API.Services.Interfaces;
using Microsoft.Extensions.Options;
using Moq;

namespace EPRN.UnitTests.API.Waste.Services
{
    [TestClass]
    public class JourneyServiceTests
    {
        private IJourneyService _journeyService;
        private Mock<IMapper> _mockMapper;
        private Mock<IRepository> _mockRepository;
        private Mock<IOptions<AppConfigSettings>> _mockConfigSettings;

        [TestInitialize]
        public void Init()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepository = new Mock<IRepository>();
            _mockConfigSettings = new Mock<IOptions<AppConfigSettings>>();

            var config = new AppConfigSettings
            {
                DeductionAmount = 100
            };

            _mockConfigSettings.Setup(m => m.Value).Returns(config);

            _journeyService = new JourneyService(
                _mockMapper.Object,
                _mockRepository.Object,
                _mockConfigSettings.Object);
        }


        [TestMethod]
        public async Task SaveWasteType_Succeeds_With_ValidIds()
        {
            // arrange
            int journeyId = 5;
            int wasteTypeId = 45;

            // act
            await _journeyService.SaveWasteType(journeyId, wasteTypeId);

            // assert
            _mockRepository.Verify(r => 
                r.UpdateJourneyWasteTypeId(
                    It.Is<int>(p => p == journeyId), 
                    It.Is<int>(p => p == wasteTypeId)), 
                Times.Once);
        }

        [TestMethod]
        public async Task SaveSelectedMonth_Succeeds_With_ValidIds_ReprocessedIt()
        {
            // arrange
            int journeyId = 8;
            int selectedMonth = 11;

            // act
            await _journeyService.SaveSelectedMonth(journeyId, selectedMonth);

            // assert
            _mockRepository.Verify(r => 
                r.UpdateJourneyMonth(
                    It.Is<int>(p => p == journeyId), 
                    It.Is<int>(p => p == selectedMonth)), 
                Times.Once);
        }

        [TestMethod]
        public async Task SaveSelectedMonth_Succeeds_With_ValidIds_SentItOn()
        {
            // arrange
            int journeyId = 9;
            int selectedMonth = 1;

            // act
            await _journeyService.SaveSelectedMonth(journeyId, selectedMonth);

            // assert
            _mockRepository.Verify(r => 
                r.UpdateJourneyMonth(
                    It.Is<int>(p => p == journeyId),
                    It.Is<int>(p => p== selectedMonth)), 
                Times.Once);
        }

        [TestMethod]
        public async Task TestGetWasteType_Succeeeds_With_Valid_Id()
        {
            // arrange
            var journeyId = 8;
            
            // act
            var result = await _journeyService.GetWasteType(journeyId);

            // assert
            _mockRepository.Verify(r => 
                r.GetWasteTypeName(
                    It.Is<int>(p => p == journeyId)), 
                Times.Once());
        }

        [TestMethod]
        public async Task SaveTonnage_WithValidParameters_Succeeds()
        {
            // arrange
            var journeyId = 5;
            var tonnage = (double)56.3;
            
            // act
            await _journeyService.SaveTonnage(journeyId, tonnage);

            // assert
            _mockRepository.Verify(r => 
                r.UpdateJourneyTonnage(
                    It.Is<int>(p => p == journeyId),
                    It.Is<double>(p => p == tonnage)), 
                Times.Once);
        }

        [TestMethod]
        public async Task GetWhatHaveYouDoneWaste_Succeeeds_With_Valid_Id_ReprocessedIt()
        {
            // arrange
            var journeyId = 8;
            
            // act
            await _journeyService.GetWhatHaveYouDoneWaste(journeyId);

            // assert
            _mockRepository.Verify(r => 
                r.GetDoneWaste(
                    It.Is<int>(p => p == journeyId)), 
                Times.Once());
        }

        [TestMethod]
        public async Task SaveReProcessor_WithInvalidJourneyId_ThrowsArgumentNullException()
        {
            // arrange
            var journeyId = 0;
            var siteId = 1;

            // act
            await _journeyService.SaveReprocessorExport(journeyId, siteId);

            // assert
            _mockRepository.Verify(r => 
                r.UpdateJourneySiteId(
                    It.Is<int>(p => p == journeyId),
                    It.Is<int>(p => p == siteId)), 
                Times.Once);
        }
    }
}
