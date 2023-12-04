﻿using AutoMapper;
using EPRN.Common.Dtos;
using EPRN.UnitTests.Helpers;
using Moq;
using Waste.API.Models;
using Waste.API.Repositories.Interfaces;
using Waste.API.Services;
using Waste.API.Services.Interfaces;

namespace EPRN.UnitTests.API.Services
{
    [TestClass]
    public class WasteServiceTests
    {
        private IWasteService _wasteService;
        private Mock<IMapper> _mockMapper;
        private Mock<IRepository> _mockRepository;

        [TestInitialize]
        public void Init()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepository = new Mock<IRepository>();
            _wasteService = new WasteService(
                _mockMapper.Object,
                _mockRepository.Object);
        }

        [TestMethod]
        public async Task WasteTypes_Returned_InOrder()
        {
            // Arrange
            var data = new List<WasteType>
            {
                new WasteType
                {
                    Id = 9,
                    Name = "Zoo"
                },
                new WasteType
                {
                    Id = 3,
                    Name = "Alphabetty Spaghetti"
                },
                new WasteType
                {
                    Id = 6,
                    Name = "Middle of the road"
                }
            };

            _mockRepository.Setup(c => c.List<WasteType>()).Returns(data);

            // Act
            var wasteTypes = await _wasteService.WasteTypes();

            // Assert
            _mockRepository.Verify(r => r.List<WasteType>(), Times.Once()); // test we called the expected function on the repo
            _mockMapper.Verify(m => 
                m.Map<List<WasteTypeDto>>(
                    It.Is<List<WasteType>>(p =>
                        TestHelper.CompareOrderedList(p, data, wt => wt.Name))), 
                    Times.Once()); // test that we called Map with the expected ordered list
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
            await _wasteService.SaveWasteType(journeyId, wasteTypeId);

            // assert
            _mockRepository.Verify(r => r.Update(It.Is<WasteJourney>(wj => wj == wasteJourney && wj.WasteTypeId == wasteTypeId)), Times.Once);
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
            await _wasteService.SaveTonnage(journeyId, tonnage);

            // assert
            _mockRepository.Verify(r => r.Update(It.Is<WasteJourney>(p => p.Id == journeyId && p.Tonnes == tonnage)), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task SaveTonnage_WithInvalidJourneyId_ThrowsArgumentNullException()
        {
            // arrange
            var journeyId = 5;
            var tonnage = 56.3;

            // act
            await _wasteService.SaveTonnage(journeyId, tonnage);

            // assert
            _mockRepository.Verify(r => r.Update(It.IsAny<WasteJourney>()), Times.Never);
        }
    }
}
