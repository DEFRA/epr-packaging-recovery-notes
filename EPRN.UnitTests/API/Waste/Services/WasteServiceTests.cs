using AutoMapper;
using EPRN.Common.Dtos;
using EPRN.UnitTests.Helpers;
using EPRN.Waste.API.Models;
using EPRN.Waste.API.Repositories.Interfaces;
using EPRN.Waste.API.Services;
using EPRN.Waste.API.Services.Interfaces;
using Moq;

namespace EPRN.UnitTests.API.Waste.Services
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
    }
}
