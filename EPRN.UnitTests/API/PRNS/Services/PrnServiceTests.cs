using AutoMapper;
using EPRN.PRNS.API.Configuration;
using EPRN.PRNS.API.Repositories.Interfaces;
using EPRN.PRNS.API.Services;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            _prnService = new PrnService(
                _mockMapper.Object,
                _mockRepository.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructService_ThrowsException_WhenMapperNotSupplied()
        {
            new PrnService(null, _mockRepository.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructService_ThrowsException_WhenRepositoryNotSupplied()
        {
            new PrnService(_mockMapper.Object, null);
        }

        [TestMethod]
        public async Task CreatePrnRecord_CallsServiceMethod()
        {
            // Arrange
            // Act
            await _prnService.CreatePrnRecord();

            // Assert
            _mockRepository.Verify(s => s.CreatePrnRecord(), Times.Once());
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
                Times.Once());
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
                Times.Once());
        }
    }
}
