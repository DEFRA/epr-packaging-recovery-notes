using EPRN.Waste.API.Configuration;
using EPRN.Waste.API.Services;
using Microsoft.Extensions.Options;
using Moq;
using EPRN.Common.Constants;

namespace EPRN.UnitTests.Services
{
    [TestClass]
    public class QuarterlyDatesServiceTests
    {
        private Mock<IOptions<AppConfigSettings>> _mockConfigSettings;
        private AppConfigSettings _configSettings;
        private QuarterlyDatesService _service;
        [TestInitialize]
        public void TestInitialize()
        {
            _mockConfigSettings = new Mock<IOptions<AppConfigSettings>>();
            _configSettings = new AppConfigSettings
            {
                QuarterStartMonths = new List<int> { 1, 4, 7, 10 },
                ReturnDeadlineForQuarter = new Dictionary<string, int>
                {
                    { "Q1", 21 },
                    { "Q2", 21 },
                    { "Q3", 21 },
                    { "Q4", 28 }
                }
            };
        }
        
        private void SetupMockConfigSettings()
        {
            _mockConfigSettings.Setup(x => x.Value).Returns(_configSettings);
            _service = new QuarterlyDatesService(_mockConfigSettings.Object);
        }

        [TestMethod]
        public async Task GetQuarterMonthsToDisplay_WhenCurrentMonthIsJanAndPreviousQuarterReturnSubmittedBeforeDeadline_ReturnsExpectedResult()
        {
            // Arrange
            _configSettings.CurrentMonthOverride = 1;
            _configSettings.CurrentDayOverride = 15;
            _configSettings.HasSubmittedReturnOverride = true;
            SetupMockConfigSettings();
            // Act
            var result = await _service.GetQuarterMonthsToDisplay(4, false);
            // Assert
            Assert.AreEqual(1, result.QuarterlyMonths.Count);
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(1));
            Assert.AreEqual(string.Empty, result.Notification);
        }
        
        [TestMethod]
        public async Task GetQuarterMonthsToDisplay_WhenCurrentMonthIsFebAndPreviousQuarterReturnSubmittedBeforeDeadline_ReturnsExpectedResult()
        {
            // Arrange
            _configSettings.CurrentMonthOverride = 2;
            _configSettings.CurrentDayOverride = 25;
            _configSettings.HasSubmittedReturnOverride = true;
            SetupMockConfigSettings();
            // Act
            var result = await _service.GetQuarterMonthsToDisplay(5, false);
            // Assert
            Assert.AreEqual(2, result.QuarterlyMonths.Count);
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(1));
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(2));
            Assert.AreEqual(string.Empty, result.Notification);
        }
        
        [TestMethod]
        public async Task GetQuarterMonthsToDisplay_WhenCurrentMonthIsMarchAndPreviousQuarterReturnSubmittedBeforeDeadline_ReturnsExpectedResult()
        {
            // Arrange
            _configSettings.CurrentMonthOverride = 3;
            _configSettings.CurrentDayOverride = 25;
            _configSettings.HasSubmittedReturnOverride = true;
            SetupMockConfigSettings();
            // Act
            var result = await _service.GetQuarterMonthsToDisplay(5, false);
            // Assert
            Assert.AreEqual(3, result.QuarterlyMonths.Count);
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(1));
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(2));
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(3));
            Assert.AreEqual(string.Empty, result.Notification);
        }

        [TestMethod]
        public async Task GetQuarterMonthsToDisplay_WhenCurrentMonthIsAprilAndPreviousQuarterReturnSubmittedBeforeDeadline_ReturnsExpectedResult()
        {
            // Arrange
            _configSettings.CurrentMonthOverride = 4;
            _configSettings.CurrentDayOverride = 15;
            _configSettings.HasSubmittedReturnOverride = true;
            SetupMockConfigSettings();
            // Act
            var result = await _service.GetQuarterMonthsToDisplay(4, false);
            // Assert
            Assert.AreEqual(1, result.QuarterlyMonths.Count);
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(4));
            Assert.AreEqual(string.Empty, result.Notification);
        }

        [TestMethod]
        public async Task GetQuarterMonthsToDisplay_WhenCurrentMonthIsMayAndPreviousQuarterReturnSubmittedBeforeDeadline_ReturnsExpectedResult()
        {
            // Arrange
            _configSettings.CurrentMonthOverride = 5;
            _configSettings.CurrentDayOverride = 25;
            _configSettings.HasSubmittedReturnOverride = true;
            SetupMockConfigSettings();
            // Act
            var result = await _service.GetQuarterMonthsToDisplay(5, false);
            // Assert
            Assert.AreEqual(2, result.QuarterlyMonths.Count);
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(4));
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(5));
            Assert.AreEqual(string.Empty, result.Notification);
        }

        [TestMethod]
        public async Task GetQuarterMonthsToDisplay_WhenCurrentMonthIsJuneAndPreviousQuarterReturnSubmittedBeforeDeadline_ReturnsExpectedResult()
        {
            // Arrange
            _configSettings.CurrentMonthOverride = 6;
            _configSettings.CurrentDayOverride = 25;
            _configSettings.HasSubmittedReturnOverride = true;
            SetupMockConfigSettings();
            // Act
            var result = await _service.GetQuarterMonthsToDisplay(5, false);
            // Assert
            Assert.AreEqual(3, result.QuarterlyMonths.Count);
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(4));
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(5));
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(6));
            Assert.AreEqual(string.Empty, result.Notification);
        }

        [TestMethod]
        public async Task GetQuarterMonthsToDisplay_WhenCurrentMonthIsJulyAndPreviousQuarterReturnSubmittedBeforeDeadline_ReturnsExpectedResult()
        {
            // Arrange
            _configSettings.CurrentMonthOverride = 7;
            _configSettings.CurrentDayOverride = 15;
            _configSettings.HasSubmittedReturnOverride = true;
            SetupMockConfigSettings();
            // Act
            var result = await _service.GetQuarterMonthsToDisplay(4, false);
            // Assert
            Assert.AreEqual(1, result.QuarterlyMonths.Count);
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(7));
            Assert.AreEqual(string.Empty, result.Notification);
        }

        [TestMethod]
        public async Task GetQuarterMonthsToDisplay_WhenCurrentMonthIsAugAndPreviousQuarterReturnSubmittedBeforeDeadline_ReturnsExpectedResult()
        {
            // Arrange
            _configSettings.CurrentMonthOverride = 8;
            _configSettings.CurrentDayOverride = 25;
            _configSettings.HasSubmittedReturnOverride = true;
            SetupMockConfigSettings();
            // Act
            var result = await _service.GetQuarterMonthsToDisplay(5, false);
            // Assert
            Assert.AreEqual(2, result.QuarterlyMonths.Count);
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(7));
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(8));
            Assert.AreEqual(string.Empty, result.Notification);
        }

        [TestMethod]
        public async Task GetQuarterMonthsToDisplay_WhenCurrentMonthIsSepAndPreviousQuarterReturnSubmittedBeforeDeadline_ReturnsExpectedResult()
        {
            // Arrange
            _configSettings.CurrentMonthOverride = 9;
            _configSettings.CurrentDayOverride = 25;
            _configSettings.HasSubmittedReturnOverride = true;
            SetupMockConfigSettings();
            // Act
            var result = await _service.GetQuarterMonthsToDisplay(5, false);
            // Assert
            Assert.AreEqual(3, result.QuarterlyMonths.Count);
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(7));
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(8));
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(9));
            Assert.AreEqual(string.Empty, result.Notification);
        }

        [TestMethod]
        public async Task GetQuarterMonthsToDisplay_WhenCurrentMonthIsOctAndPreviousQuarterReturnSubmittedBeforeDeadline_ReturnsExpectedResult()
        {
            // Arrange
            _configSettings.CurrentMonthOverride = 10;
            _configSettings.CurrentDayOverride = 15;
            _configSettings.HasSubmittedReturnOverride = true;
            SetupMockConfigSettings();
            // Act
            var result = await _service.GetQuarterMonthsToDisplay(4, false);
            // Assert
            Assert.AreEqual(1, result.QuarterlyMonths.Count);
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(10));
            Assert.AreEqual(string.Empty, result.Notification);
        }

        [TestMethod]
        public async Task GetQuarterMonthsToDisplay_WhenCurrentMonthIsNovAndPreviousQuarterReturnSubmittedBeforeDeadline_ReturnsExpectedResult()
        {
            // Arrange
            _configSettings.CurrentMonthOverride = 11;
            _configSettings.CurrentDayOverride = 25;
            _configSettings.HasSubmittedReturnOverride = true;
            SetupMockConfigSettings();
            // Act
            var result = await _service.GetQuarterMonthsToDisplay(5, false);
            // Assert
            Assert.AreEqual(2, result.QuarterlyMonths.Count);
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(10));
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(11));
            Assert.AreEqual(string.Empty, result.Notification);
        }

        [TestMethod]
        public async Task GetQuarterMonthsToDisplay_WhenCurrentMonthIsDecAndPreviousQuarterReturnSubmittedBeforeDeadline_ReturnsExpectedResult()
        {
            // Arrange
            _configSettings.CurrentMonthOverride = 12;
            _configSettings.CurrentDayOverride = 25;
            _configSettings.HasSubmittedReturnOverride = true;
            SetupMockConfigSettings();
            // Act
            var result = await _service.GetQuarterMonthsToDisplay(5, false);
            // Assert
            Assert.AreEqual(3, result.QuarterlyMonths.Count);
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(10));
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(11));
            Assert.IsTrue(result.QuarterlyMonths.ContainsKey(12));
            Assert.AreEqual(string.Empty, result.Notification);
        }
    }
}
