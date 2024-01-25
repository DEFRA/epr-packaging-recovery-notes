﻿using EPRN.Waste.API.Configuration;
using EPRN.Waste.API.Services;
using Microsoft.Extensions.Options;
using Moq;
using System.Globalization;
using EPRN.Common.Constants;

namespace EPRN.UnitTests.Services
{
    [TestClass]
    public class QuarterlyDatesServiceTests
    {
        private Mock<IOptions<AppConfigSettings>> _mockConfigSettings;
        private QuarterlyDatesService _service;
        
        
        [TestInitialize]
        public void TestInitialize()
        {
            _mockConfigSettings = new Mock<IOptions<AppConfigSettings>>();
            var configSettings = new AppConfigSettings
            {
                QuarterStartMonths = new List<int> { 1, 4, 7, 10 },
                ReturnDeadlineForQuarter = new Dictionary<string, int>
                {
                    { "Q1", 21 },
                    { "Q2", 21 },
                    { "Q3", 21 },
                    { "Q4", 28 }
                },
                CurrentMonthOverride = 1,
                CurrentDayOverride = 1,
                HasSubmittedReturnOverride = false,
                DeductionAmount = 0.15,

            };
            _mockConfigSettings.Setup(x => x.Value).Returns(configSettings);
            _service = new QuarterlyDatesService(_mockConfigSettings.Object);
        }


        [TestMethod]
        public async Task GetQuarterMonthsToDisplay_WhenCurrentMonthIsJanuaryAndPreviousQuarterReturnNotSubmitted_ReturnsExpectedResult()
        {
            // Arrange
            var configSettings = new AppConfigSettings
            {
                QuarterStartMonths = new List<int> { 1, 4, 7, 10 },
                ReturnDeadlineForQuarter = new Dictionary<string, int>
                {
                    { "Q1", 21 },
                    { "Q2", 21 },
                    { "Q3", 21 },
                    { "Q4", 28 }
                },
                CurrentMonthOverride = 1,
                CurrentDayOverride = 1,
                HasSubmittedReturnOverride = false
            };
            _mockConfigSettings.Setup(x => x.Value).Returns(configSettings);
            
            // Act
            var result = await _service.GetQuarterMonthsToDisplay(1, false);
            // Assert
            Assert.AreEqual(4, result.QuarterlyMonths.Count);
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(10));
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(11));
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(12));
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(1));
        }

        [TestMethod]
        public async Task GetQuarterMonthsToDisplay_WhenCurrentMonthIsFebruaryAndPreviousQuarterReturnNotSubmitted_ReturnsExpectedResult()
        {
            // Arrange
            var configSettings = new AppConfigSettings
            {
                QuarterStartMonths = new List<int> { 1, 4, 7, 10 },
                ReturnDeadlineForQuarter = new Dictionary<string, int>
                {
                    { "Q1", 21 },
                    { "Q2", 21 },
                    { "Q3", 21 },
                    { "Q4", 28 }
                },
                CurrentMonthOverride = 2,
                CurrentDayOverride = 1,
                HasSubmittedReturnOverride = false
            };
            _mockConfigSettings.Setup(x => x.Value).Returns(configSettings);
            // Act
            var result = await _service.GetQuarterMonthsToDisplay(2, false);
            // Assert
            Assert.AreEqual(5, result.QuarterlyMonths.Count);
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(10));
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(11));
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(12));
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(1));
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(2));
            Assert.AreEqual(Strings.Notifications.QuarterlyReturnDue, result.Notification);
        }

        [TestMethod]
        public async Task GetQuarterMonthsToDisplay_WhenCurrentMonthIsAprilAndPreviousQuarterReturnNotSubmitted_ReturnsExpectedResult()
        {
            // Arrange
            var configSettings = new AppConfigSettings
            {
                QuarterStartMonths = new List<int> { 1, 4, 7, 10 },
                ReturnDeadlineForQuarter = new Dictionary<string, int>
                {
                    { "Q1", 21 },
                    { "Q2", 21 },
                    { "Q3", 21 },
                    { "Q4", 28 }
                },
                CurrentMonthOverride = 4,
                CurrentDayOverride = 1,
                HasSubmittedReturnOverride = false
            };
            _mockConfigSettings.Setup(x => x.Value).Returns(configSettings);
            // Act
            var result = await _service.GetQuarterMonthsToDisplay(3, false);
            // Assert
            Assert.AreEqual(4, result.QuarterlyMonths.Count);
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(1));
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(2));
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(3));
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(4));
            Assert.AreEqual(Strings.Notifications.QuarterlyReturnDue, result.Notification);
        }

        [TestMethod]
        public async Task GetQuarterMonthsToDisplay_WhenCurrentMonthIsJulyAndPreviousQuarterReturnNotSubmitted_ReturnsExpectedResult()
        {
            // Arrange
            var configSettings = new AppConfigSettings
            {
                QuarterStartMonths = new List<int> { 1, 4, 7, 10 },
                ReturnDeadlineForQuarter = new Dictionary<string, int>
                {
                    { "Q1", 21 },
                    { "Q2", 21 },
                    { "Q3", 21 },
                    { "Q4", 28 }
                },
                CurrentMonthOverride = 7,
                CurrentDayOverride = 1,
                HasSubmittedReturnOverride = false
            };
            _mockConfigSettings.Setup(x => x.Value).Returns(configSettings);
            // Act
            var result = await _service.GetQuarterMonthsToDisplay(3, false);
            // Assert
            Assert.AreEqual(4, result.QuarterlyMonths.Count);
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(4));
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(5));
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(6));
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(7));
            Assert.AreEqual(Strings.Notifications.QuarterlyReturnDue, result.Notification);
        }
               [TestMethod]
        public async Task GetQuarterMonthsToDisplay_WhenCurrentMonthIsOctoberAndPreviousQuarterReturnNotSubmitted_ReturnsExpectedResult()
        {
            // Arrange
            var configSettings = new AppConfigSettings
            {
                QuarterStartMonths = new List<int> { 1, 4, 7, 10 },
                ReturnDeadlineForQuarter = new Dictionary<string, int>
                {
                    { "Q1", 21 },
                    { "Q2", 21 },
                    { "Q3", 21 },
                    { "Q4", 28 }
                },
                CurrentMonthOverride = 10,
                CurrentDayOverride = 1,
                HasSubmittedReturnOverride = false
            };
            _mockConfigSettings.Setup(x => x.Value).Returns(configSettings);
            // Act
            var result = await _service.GetQuarterMonthsToDisplay(3, false);
            // Assert
            Assert.AreEqual(4, result.QuarterlyMonths.Count);
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(7));
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(8));
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(9));
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(10));
            Assert.AreEqual(Strings.Notifications.QuarterlyReturnDue, result.Notification);
        } 
    }
}
