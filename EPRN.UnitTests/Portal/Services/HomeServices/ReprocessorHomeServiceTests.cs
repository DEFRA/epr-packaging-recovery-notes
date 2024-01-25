using EPRN.Common.Dtos;
using EPRN.Common.Enums;
using EPRN.Portal.Configuration;
using EPRN.Portal.Resources;
using EPRN.Portal.RESTServices.Interfaces;
using EPRN.Portal.Services.HomeServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;

namespace EPRN.UnitTests.Portal.Services.HomeServices
{
    [TestClass]
    public class ReprocessorHomeServiceTests
    {
        private Mock<IHttpJourneyService> _mockHttpJourneyService;
        private UserBasedReprocessorService _reprocessorHomeService;

        [TestInitialize]
        public void Init()
        {
            var mockConfig = new AppConfigSettings
            {
                DeductionAmount_Exporter = 2.34,
                DeductionAmount_ExporterAndReprocessor = 4.56,
                DeductionAmount_Reprocessor = 2.5
            };

            var mockUrlHelper = new Mock<IUrlHelper>();

            var mockConfigSettings = new Mock<IOptions<AppConfigSettings>>();
            mockConfigSettings.Setup(o => o.Value).Returns(mockConfig);

            _mockHttpJourneyService = new Mock<IHttpJourneyService>();

            _reprocessorHomeService = new UserBasedReprocessorService(
                mockConfigSettings.Object,
                _mockHttpJourneyService.Object
                );
        }

        [TestMethod]
        public async Task GetCheckAnswers_ReturnsViewModel_WhenJourneyDtoIsNotNull()
        {
            int journeyId = 1;
            var journeyAnswersDto = new JourneyAnswersDto
            {
                Month = 1,
                Tonnes = 23,
                BaledWithWire = false,
                Adjustment = 34.5,
                Note = "Note",
                WasteType = "Paper",
                WasteSubType = "Mixed paper/board",
                DoneWaste = DoneWaste.ReprocessedIt,
                Completed = true
            };

            _mockHttpJourneyService.Setup(service => service.GetJourneyAnswers(journeyId)).ReturnsAsync(journeyAnswersDto);

            // Act
            var result = await _reprocessorHomeService.GetCheckAnswers(journeyId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(journeyId, result.JourneyId);
            Assert.AreEqual(journeyAnswersDto.Completed, result.Completed);

            _mockHttpJourneyService.Verify(service => service.GetJourneyAnswers(journeyId), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task GetCheckAnswers_ThrowsNullReferenceException_WhenJourneyDtoIsNull()
        {
            // Arrange
            int journeyId = 1;
            _mockHttpJourneyService.Setup(service => service.GetJourneyAnswers(journeyId)).ReturnsAsync((JourneyAnswersDto)null);

            // Act
            await _reprocessorHomeService.GetCheckAnswers(journeyId);

            // Assert
            // Expecting a NullReferenceException here

            _mockHttpJourneyService.Verify(service => service.GetJourneyAnswers(journeyId), Times.Once());
        }

        [TestMethod]
        public async Task GetJourneyAnswers_ReturnsValidViewModel_WhenWhatDoneWithWasteIsReprocessed()
        {
            // Arrange
            int journeyId = 1;
            var journeyAnswersDto = new JourneyAnswersDto
            {
                JourneyId = journeyId,
                Month = 1,
                Tonnes = 23,
                BaledWithWire = false,
                Adjustment = 34.5,
                Note = "Note",
                WasteType = "Paper",
                WasteSubType = "Mixed paper/board",
                DoneWaste = DoneWaste.ReprocessedIt,
                Completed = true
            };

            _mockHttpJourneyService.Setup(service => service.GetJourneyAnswers(journeyId)).ReturnsAsync(journeyAnswersDto);

            // Act
            var result = await _reprocessorHomeService.GetCheckAnswers(journeyId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(journeyAnswersDto.JourneyId, result.JourneyId);

            _mockHttpJourneyService.Verify(service => service.GetJourneyAnswers(journeyId), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task GetCheckAnswers_ReturnsException_WhenWhatDoneWithWasteIsNotKnown()
        {
            // Arrange
            int journeyId = 1;
            var journeyAnswersDto = new JourneyAnswersDto
            {
                Month = 1,
                Tonnes = 23,
                BaledWithWire = false,
                Adjustment = 34.5,
                Note = "Note",
                WasteType = "Paper",
                WasteSubType = "Mixed paper/board",
                //DoneWaste = null,
                Completed = true
            };

            _mockHttpJourneyService.Setup(service => service.GetJourneyAnswers(journeyId)).ReturnsAsync(journeyAnswersDto);

            // Act
            var result = await _reprocessorHomeService.GetCheckAnswers(journeyId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(journeyAnswersDto.JourneyId, result.JourneyId);

            _mockHttpJourneyService.Verify(service => service.GetJourneyAnswers(journeyId), Times.Once());

        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task GetCheckAnswers_ThrowsNullReferenceException_WhenWhatDoneWithWasteIsNull()
        {
            // Arrange
            int journeyId = 1;
            var journeyAnswersDto = new JourneyAnswersDto
            {
                //DoneWaste = null
            };

            _mockHttpJourneyService.Setup(service => service.GetJourneyAnswers(journeyId)).ReturnsAsync(journeyAnswersDto);

            // Act
            await _reprocessorHomeService.GetCheckAnswers(journeyId);

            // Assert
            // Expecting a NullReferenceException here

            _mockHttpJourneyService.Verify(service => service.GetJourneyAnswers(journeyId), Times.Once());
        }

        [TestMethod]
        public async Task GetCheckAnswers_ReturnsViewModel_WhenJourneyIdIsValid()
        {
            int journeyId = 1;
            var journeyAnswersDto = new JourneyAnswersDto
            {
                Month = 1,
                Tonnes = 23,
                BaledWithWire = false,
                Adjustment = 34.5,
                Note = "Note",
                WasteType = "Paper",
                WasteSubType = "Mixed paper/board",
                DoneWaste = DoneWaste.ReprocessedIt,
                Completed = true
            };

            _mockHttpJourneyService.Setup(service => service.GetJourneyAnswers(journeyId)).ReturnsAsync(journeyAnswersDto);

            // Act
            var result = await _reprocessorHomeService.GetCheckAnswers(journeyId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(journeyId, result.JourneyId);

            _mockHttpJourneyService.Verify(service => service.GetJourneyAnswers(journeyId), Times.Once());
        }
    }
}
