using EPRN.Common.Dtos;
using EPRN.Portal.Configuration;
using EPRN.Portal.Resources;
using EPRN.Portal.RESTServices.Interfaces;
using EPRN.Portal.Services.HomeServices;
using EPRN.Portal.ViewModels.Waste;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;
using Moq;

namespace EPRN.UnitTests.Portal.Services.HomeServices
{
    [TestClass]
    public class ReprocessorHomeServiceTests
    {
        private Mock<IHttpJourneyService> _mockHttpJourneyService;
        private ReprocessorHomeService _reprocessorHomeService;

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
            var mockUrlHelperFactory = new Mock<IUrlHelperFactory>();
            var mockActionContextAccessor = new Mock<IActionContextAccessor>();
            mockUrlHelperFactory.Setup(f => f.GetUrlHelper(It.IsAny<ActionContext>())).Returns(mockUrlHelper.Object);

            _mockHttpJourneyService = new Mock<IHttpJourneyService>();

            _reprocessorHomeService = new ReprocessorHomeService(
                mockConfigSettings.Object,
                _mockHttpJourneyService.Object,
                mockUrlHelperFactory.Object,
                mockActionContextAccessor.Object
                );
        }

        [TestMethod]
        public async Task GetCheckAnswers_ReturnsViewModel_WhenJourneyDtoIsNotNull()
        {
            int journeyId = 1;
            var journeyAnswersDto = new JourneyAnswersDto
            {
                Month = "1",
                Tonnes = "23",
                BaledWithWire = "No",
                TonnageAdjusted = "34.5",
                Note = "Note",
                WasteType = "Paper",
                WasteSubType = "Mixed paper/board",
                WhatDoneWithWaste = "ReprocessedIt",
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
        public async Task GetCheckAnswers_ReturnsViewModelWithCorrectSections_WhenWhatDoneWithWasteIsReprocessed()
        {
            // Arrange
            int journeyId = 1;
            var journeyAnswersDto = new JourneyAnswersDto
            {
                Month = "1",
                Tonnes = "23",
                BaledWithWire = "No",
                TonnageAdjusted = "34.5",
                Note = "Note",
                WasteType = "Paper",
                WasteSubType = "Mixed paper/board",
                WhatDoneWithWaste = "ReprocessedIt",
                Completed = true
            };

            var expectedSection = new Dictionary<string, List<CheckAnswerViewModel>>();

            var rows = new List<CheckAnswerViewModel>
            {
                new CheckAnswerViewModel { Question = CYAResources.TypeOfWaste, Answer = "Example subtype", ChangeLink = "subtypeUrl" },
                new CheckAnswerViewModel { Question = CYAResources.BaledWithWire, Answer = "Example baled", ChangeLink = "baledUrl" },
                new CheckAnswerViewModel { Question = CYAResources.TonnageOfWaste, Answer = "Example tonnage", ChangeLink = "tonnageUrl" },
                new CheckAnswerViewModel { Question = CYAResources.TonnageAdjusted, Answer = "Example adjusted", ChangeLink = "adjustedUrl" },
                new CheckAnswerViewModel { Question = CYAResources.MonthReceived, Answer = "Example month", ChangeLink = "monthUrl" },
                new CheckAnswerViewModel { Question = CYAResources.Note, Answer = "Example note", ChangeLink = "noteUrl" }
            };

            expectedSection.Add(CYAResources.Title, rows);

            _mockHttpJourneyService.Setup(service => service.GetJourneyAnswers(journeyId)).ReturnsAsync(journeyAnswersDto);

            // Act
            var result = await _reprocessorHomeService.GetCheckAnswers(journeyId);

            // Assert
            Assert.AreEqual(expectedSection.Count, result.Sections.Count);

            _mockHttpJourneyService.Verify(service => service.GetJourneyAnswers(journeyId), Times.Once());
        }

        [TestMethod]
        public async Task GetCheckAnswers_ReturnsViewModelWithCorrectSections_WhenWhatDoneWithWasteIsSetOn()
        {
            // Arrange
            int journeyId = 1;
            var journeyAnswersDto = new JourneyAnswersDto
            {
                Month = "1",
                Tonnes = "23",
                BaledWithWire = "No",
                TonnageAdjusted = "34.5",
                Note = "Note",
                WasteType = "Paper",
                WasteSubType = "Mixed paper/board",
                WhatDoneWithWaste = "SentItOn",
                Completed = true
            };

            var expectedSection = new Dictionary<string, List<CheckAnswerViewModel>>();

            var rows = new List<CheckAnswerViewModel>
            {
                new CheckAnswerViewModel { Question = CYAResources.ReprocessorWhereWasteSentName, Answer = "Example name", ChangeLink = "nameUrl" },
                new CheckAnswerViewModel { Question = CYAResources.ReprocessorWhereWasteSentAddress, Answer = "Example address", ChangeLink = "addressUrl" },
                new CheckAnswerViewModel { Question = CYAResources.TypeOfWaste, Answer = "Example subtype", ChangeLink = "subtypeUrl" },
                new CheckAnswerViewModel { Question = CYAResources.BaledWithWire, Answer = "Example baled", ChangeLink = "baledUrl" },
                new CheckAnswerViewModel { Question = CYAResources.TonnageOfWaste, Answer = "Example tonnage", ChangeLink = "tonnageUrl" },
                new CheckAnswerViewModel { Question = CYAResources.TonnageAdjusted, Answer = "Example adjusted", ChangeLink = "adjustedUrl" },
                new CheckAnswerViewModel { Question = CYAResources.MonthReceived, Answer = "Example month", ChangeLink = "monthUrl" },
                new CheckAnswerViewModel { Question = CYAResources.Note, Answer = "Example note", ChangeLink = "noteUrl" }
            };

            expectedSection.Add(CYAResources.Title, rows);

            _mockHttpJourneyService.Setup(service => service.GetJourneyAnswers(journeyId)).ReturnsAsync(journeyAnswersDto);

            // Act
            var result = await _reprocessorHomeService.GetCheckAnswers(journeyId);

            // Assert
            Assert.AreEqual(expectedSection.Count, result.Sections.Count);
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
                WhatDoneWithWaste = null
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
                Month = "1",
                Tonnes = "23",
                BaledWithWire = "No",
                TonnageAdjusted = "34.5",
                Note = "Note",
                WasteType = "Paper",
                WasteSubType = "Mixed paper/board",
                WhatDoneWithWaste = "ReprocessedIt",
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
