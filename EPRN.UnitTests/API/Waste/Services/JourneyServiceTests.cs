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

            var wasteJourney = new WasteJourney
            {
            };

            _mockRepository.Setup(r => r.GetById<WasteJourney>(It.Is<int>(p => p == journeyId))).ReturnsAsync(wasteJourney);

            // act
            await _journeyService.SaveWasteType(journeyId, wasteTypeId);

            // assert
            _mockRepository.Verify(r => r.Update(It.Is<WasteJourney>(wj => wj == wasteJourney && wj.WasteTypeId == wasteTypeId)), Times.Once);
        }

        [TestMethod]
        public async Task SaveSelectedMonth_Succeeds_With_ValidIds_ReprocessedIt()
        {
            // arrange
            int journeyId = 8;
            int selectedMonth = 11;
            var wasteJourney = new WasteJourney { };

            wasteJourney.DoneWaste = DoneWaste.ReprocessedIt;

            _mockRepository.Setup(r => r.GetById<WasteJourney>(It.Is<int>(p => p == journeyId))).ReturnsAsync(wasteJourney);

            // act
            await _journeyService.SaveSelectedMonth(journeyId, selectedMonth);

            // assert
            _mockRepository.Verify(r => r.Update(It.Is<WasteJourney>(wj => wj == wasteJourney && wj.Month == selectedMonth)), Times.Once);
        }

        [TestMethod]
        public async Task SaveSelectedMonth_Succeeds_With_ValidIds_SentItOn()
        {
            // arrange
            int journeyId = 9;
            int selectedMonth = 1;
            var wasteJourney = new WasteJourney { };

            wasteJourney.DoneWaste = DoneWaste.SentItOn;

            _mockRepository.Setup(r => r.GetById<WasteJourney>(It.Is<int>(p => p == journeyId))).ReturnsAsync(wasteJourney);

            // act
            await _journeyService.SaveSelectedMonth(journeyId, selectedMonth);

            // assert
            _mockRepository.Verify(r => r.Update(It.Is<WasteJourney>(wj => wj == wasteJourney && wj.Month == selectedMonth)), Times.Once);
        }

        [TestMethod]
        public async Task TestGetWasteType_Succeeeds_With_Valid_Id()
        {
            // arrange
            var journeyId = 8;
            var expectedWasteType = "testWasteType";
            var wasteJourney = new WasteJourney
            {
                Id = journeyId,
                WasteTypeId = 45,
                WasteType = new WasteType()
                {
                    Name = expectedWasteType
                }
            };

            _mockRepository.Setup(r => r.GetById<WasteJourney>(It.Is<int>(p => p == journeyId))).ReturnsAsync(wasteJourney);

            // act
            var result = await _journeyService.GetWasteType(journeyId);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string));
            Assert.AreEqual(result, expectedWasteType);
            _mockRepository.Verify(r => r.GetById<WasteJourney>(It.Is<int>(p => p == journeyId)), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task TestGetWasteType_Fails_With_InValid_JourneyRecord()
        {
            // Arrange
            var journeyId = 8;
            var wasteJourney = new WasteJourney { };

            _mockRepository.Setup(r => r.GetById<WasteJourney>(It.Is<int>(p => p == journeyId))).ReturnsAsync(wasteJourney);

            // Act
            var result = await _journeyService.GetWasteType(journeyId);

            //Assert
            Assert.IsNotNull(wasteJourney);
            _mockRepository.Verify(r => r.GetById<WasteJourney>(It.Is<int>(p => p == journeyId)), Times.Never());

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task TestGetWasteType_Fails_With_InValid_WasteTypeId()
        {
            // arrange
            var journeyId = 8;
            var expectedWasteType = "testWasteType";
            var wasteJourney = new WasteJourney
            {
                Id = journeyId,
                WasteTypeId = null,
                WasteType = new WasteType()
                {
                    Name = expectedWasteType
                }
            };

            _mockRepository.Setup(r => r.GetById<WasteJourney>(It.Is<int>(p => p == journeyId))).ReturnsAsync(wasteJourney);

            // Act
            var result = await _journeyService.GetWasteType(journeyId);

            //Assert
            Assert.IsNotNull(wasteJourney.WasteTypeId);
            _mockRepository.Verify(r => r.GetById<WasteJourney>(It.Is<int>(p => p == journeyId)), Times.Never());

        }

        [TestMethod]
        public async Task SaveTonnage_WithValidParameters_Succeeds()
        {
            // arrange
            var journeyId = 5;
            var tonnage = 56.3;
            _mockRepository.Setup(r => r.GetById<WasteJourney>(journeyId)).ReturnsAsync(new WasteJourney
            {
                Id = journeyId,
            });

            // act
            await _journeyService.SaveTonnage(journeyId, tonnage);

            // assert
            _mockRepository.Verify(r => r.Update(It.Is<WasteJourney>(p => p.Id == journeyId && p.Tonnes == tonnage)), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task SaveTonnage_WithInvalidJourneyId_ThrowsArgumentNullException()
        {
            // arrange
            var journeyId = 5;
            var tonnage = 56.3;

            // act
            await _journeyService.SaveTonnage(journeyId, tonnage);

            // assert
            _mockRepository.Verify(r => r.Update(It.IsAny<WasteJourney>()), Times.Never);
        }

        [TestMethod]
        public async Task GetWhatHaveYouDoneWaste_Succeeeds_With_Valid_Id_ReprocessedIt()
        {
            // arrange
            var journeyId = 8;
            var expectedWhatHaveYouDoneWaste = DoneWaste.ReprocessedIt;
            var wasteJourney = new WasteJourney
            {
                Id = journeyId,
                DoneWaste = expectedWhatHaveYouDoneWaste
            };

            _mockRepository.Setup(r => r.GetById<WasteJourney>(It.Is<int>(p => p == journeyId))).ReturnsAsync(wasteJourney);

            // act
            var result = await _journeyService.GetWhatHaveYouDoneWaste(journeyId);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DoneWaste));
            Assert.AreEqual(result, expectedWhatHaveYouDoneWaste);
            _mockRepository.Verify(r => r.GetById<WasteJourney>(It.Is<int>(p => p == journeyId)), Times.Once());
        }

        [TestMethod]
        public async Task GetWhatHaveYouDoneWaste_Succeeeds_With_Valid_Id_SentItOn()
        {
            // arrange
            var journeyId = 8;
            var expectedWhatHaveYouDoneWaste = DoneWaste.SentItOn;
            var wasteJourney = new WasteJourney
            {
                Id = journeyId,
                DoneWaste = expectedWhatHaveYouDoneWaste
            };

            _mockRepository.Setup(r => r.GetById<WasteJourney>(It.Is<int>(p => p == journeyId))).ReturnsAsync(wasteJourney);

            // act
            var result = await _journeyService.GetWhatHaveYouDoneWaste(journeyId);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DoneWaste));
            Assert.AreEqual(result, expectedWhatHaveYouDoneWaste);
            _mockRepository.Verify(r => r.GetById<WasteJourney>(It.Is<int>(p => p == journeyId)), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task TestGetWhatHaveYouyDoneWaste_Fails_With_InValid_JourneyRecord()
        {
            // Arrange
            var journeyId = 8;
            var wasteJourney = new WasteJourney { };

            _mockRepository.Setup(r => r.GetById<WasteJourney>(It.Is<int>(p => p == journeyId))).ReturnsAsync(wasteJourney);

            // Act
            var result = await _journeyService.GetWhatHaveYouDoneWaste(journeyId);

            //Assert
            Assert.IsNotNull(wasteJourney);
            _mockRepository.Verify(r => r.GetById<WasteJourney>(It.Is<int>(p => p == journeyId)), Times.Never());

        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task SaveReProcessor_WithInvalidJourneyId_ThrowsArgumentNullException()
        {
            // arrange
            var journeyId = 0;
            var siteId = 1;

            // act
            await _wasteService.SaveReprocessorExport(journeyId, siteId);

            // assert
            _mockRepository.Verify(r => r.Update(It.IsAny<WasteJourney>()), Times.Never);
        }

        [TestMethod]
        public async Task SaveReProcessor_WithValidParameters_Succeeds()
        {
            // arrange
            var journeyId = 5;
            var siteId = 1;
            _mockRepository.Setup(r => r.GetById<WasteJourney>(journeyId)).ReturnsAsync(new WasteJourney
            {
                Id = journeyId,
            });

            // act
            await _wasteService.SaveReprocessorExport(journeyId, siteId);

            // assert
            _mockRepository.Verify(r => r.Update(It.Is<WasteJourney>(p => p.Id == journeyId && p.SiteId == siteId)), Times.Once);
        }


    }
}
