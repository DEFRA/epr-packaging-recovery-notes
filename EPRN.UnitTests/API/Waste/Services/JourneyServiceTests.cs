using AutoMapper;
using EPRN.Common.Data.DataModels;
using EPRN.Common.Dtos;
using EPRN.Waste.API.Configuration;
using EPRN.Waste.API.Repositories.Interfaces;
using EPRN.Waste.API.Services;
using EPRN.Waste.API.Services.Interfaces;
using Microsoft.Extensions.Options;
using Moq;
using System.Reflection.Metadata;

namespace EPRN.UnitTests.API.Waste.Services
{
    [TestClass]
    public class JourneyServiceTests
    {
        private IJourneyService _journeyService;
        private Mock<IMapper> _mockMapper;
        private Mock<IRepository> _mockRepository;
        private Mock<IOptions<AppConfigSettings>> _mockConfigSettings;
        private const double AccredidationLimit = 400;

        [TestInitialize]
        public void Init()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepository = new Mock<IRepository>();
            _mockConfigSettings = new Mock<IOptions<AppConfigSettings>>();

            var config = new AppConfigSettings
            {
                DeductionAmount = 100,
                AccredidationLimit = AccredidationLimit
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
                    It.Is<int>(p => p == selectedMonth)),
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
        public async Task SaveNote_WithValidParameters_Succeeds()
        {
            // arrange
            var journeyId = 5;
            var note = "abc";
            
            // act
            await _journeyService.SaveWasteRecordNote(journeyId, note);

            // assert
            _mockRepository.Verify(r =>
                r.UpdateWasteNote(
                    It.Is<int>(p => p == journeyId),
                    It.Is<string>(p => p == note)),
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
        public async Task GetWasteRecordNote_ReturnsValidDto_With_ValidJourneyId()
        {
            // arrange
            var journeyId = 8;
            var noteDto = new NoteDto() { Id = journeyId, Note = "abc", WasteCategory = Common.Enums.Category.Unknown };
            _mockRepository.Setup(x => x.GetWasteNote(It.IsAny<int>())).ReturnsAsync(noteDto);

            // act
            var dto = await _journeyService.GetWasteRecordNote(journeyId);

            // assert
            Assert.IsNotNull(dto);
            
            _mockRepository.Verify(r =>
                r.GetWasteNote(
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

        [TestMethod]
        public async Task GetJourneyAnswers_WhenJourneyExists_ReturnsJourneyAnswersDto()
        {
            // Arrange
            int journeyId = 1;
            var journeyDto = new JourneyAnswersDto
            {
                Month = 1,
                Tonnes = 34.7,
                BaledWithWire = false,
                Note = "",
                Completed = true,
            };

            _mockRepository.Setup(repo => repo.GetWasteJourneyAnswersById(journeyId)).ReturnsAsync(journeyDto);

            // Act
            var result = await _journeyService.GetJourneyAnswers(journeyId);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetJourneyAnswers_WhenJourneyIsNull_ReturnsANullObject()
        {
            // Arrange
            int journeyId = 1;
            _mockRepository.Setup(repo => repo.GetWasteJourneyById_FullModel(journeyId)).ReturnsAsync((WasteJourney)null);

            // Act & Assert
            var result = await _journeyService.GetJourneyAnswers(journeyId);

            // Assert
            Assert.IsNull(result);
        }


        [TestMethod]
        public async Task GetAccredidationLimit_Returns_With_Valid_UserReference()
        {
            // arrange
            var userReferenceId = "someuser";
            var newQuantityEntered = 10;
            var existingTotalQuantity = 200;

            _mockRepository.Setup(x => x.GetTotalQuantityForAllUserJourneys(userReferenceId)).ReturnsAsync(existingTotalQuantity);

            // act
            var result = await _journeyService.GetAccredidationLimit(userReferenceId, newQuantityEntered);

            // assert
            _mockRepository.Verify(r =>
                r.GetTotalQuantityForAllUserJourneys(
                    It.Is<string>(p => p == userReferenceId)),
                Times.Once());

            Assert.IsTrue(result.ExcessOfLimit == 190);
        }

        [TestMethod]
        public async Task GetAccredidationLimit_Returns_With_InValid_UserReference()
        {
            // arrange
            var userReferenceId = "no user";
            var newQuantityEntered = 10;
            double? returnedValue = null;

            _mockRepository.Setup(x => x.GetTotalQuantityForAllUserJourneys(userReferenceId)).ReturnsAsync(returnedValue);

            // act
            var result = await _journeyService.GetAccredidationLimit(userReferenceId, newQuantityEntered);

            // assert
            _mockRepository.Verify(r =>
                r.GetTotalQuantityForAllUserJourneys(
                    It.Is<string>(p => p == userReferenceId)),
                Times.Once());

            Assert.IsTrue(result.ExcessOfLimit == (AccredidationLimit - newQuantityEntered));
        }

    }
}
