using AutoMapper;
using EPRN.UnitTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Waste.API.Models;
using Waste.API.Services;
using Waste.API.Services.Interfaces;
using WasteManagement.API.Data;

namespace EPRN.UnitTests.API.Services
{
    [TestClass]
    public class WasteServiceTests
    {
        private IWasteService _wasteService;
        private Mock<IMapper> _mockMapper;
        private Mock<WasteContext> _mockContext;

        [TestInitialize]
        public void Init()
        {
            _mockMapper = new Mock<IMapper>();
            _mockContext = new Mock<WasteContext>();
            _wasteService = new WasteService(
                _mockMapper.Object,
                _mockContext.Object);
        }

        [TestMethod]
        public async Task WasteTypes_Returned_InOrder()
        {
            // Arrange
            var data = new List<WasteType>
            {
            }.AsAsyncQueryable();

            // Act

            // Assert


            var mockSet = new Mock<DbSet<WasteType>>();
            mockSet.As<IQueryable<WasteType>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<WasteType>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<WasteType>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<WasteType>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            _mockContext.Setup(c => c.WasteTypes).Returns(mockSet.Object);

            var wasteTypes = await _wasteService.WasteTypes();
        }
    }
}
