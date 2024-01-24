using EPRN.Waste.API.Configuration;
using EPRN.Waste.API.Services;
using Microsoft.Extensions.Options;
using Moq;
using System.Globalization;
namespace EPRN.UnitTests.Services
{
    [TestClass]
    public class QuarterlyDatesServiceTests
    {
    //    private Mock<IOptions<AppConfigSettings>> _mockConfigSettings;
    //    private QuarterlyDatesService _service;
    //    [TestInitialize]
    //    public void Setup()
    //    {
    //        _mockConfigSettings = new Mock<IOptions<AppConfigSettings>>();
    //        _mockConfigSettings.Setup(x => x.Value).Returns(new AppConfigSettings());
    //        _mockConfigSettings.Setup(x => x.Value).Returns(new AppConfigSettings
    //        {
    //            QuarterStartMonths = new List<int> { 1, 4, 7, 10 },
    //            ReturnDeadlineDays = new List<int> { 21, 21, 21, 28 }
    //        });
    //        _service = new QuarterlyDatesService(_mockConfigSettings.Object);
    //    }
    //    [TestMethod]
    //    public async Task GetQuarterMonthsToDisplay_WithinFirstMonthOfCurrentQuarterAndReturnSubmitted_ReturnsFirstMonthOfCurrentQuarter()
    //    {
    //        // Arrange
    //        var currentDate = new DateTime(DateTime.Now.Year, 1, 15); // Assuming the current date is within the first month of the first quarter
    //        var hasSubmittedPreviousQuarterReturn = true;
    //        // Act
    //        var result = await _service.GetQuarterMonthsToDisplay(currentDate.Month, hasSubmittedPreviousQuarterReturn);
    //        // Assert
    //        Assert.AreEqual(1, result.QuarterlyMonths.Count);
    //        Assert.IsTrue(result.QuarterlyMonths.ContainsKey(1));
    //        Assert.AreEqual(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(currentDate.Month), result.QuarterlyMonths[1]);
    //    }
    //    [TestMethod]
    //    public async Task GetQuarterMonthsToDisplay_WithinFirstMonthOfCurrentQuarterAndReturnSubmitted_ReturnsMonth_1_and_2_OfCurrentQuarter()
    //    {
    //        // Arrange
    //        var currentDate = new DateTime(DateTime.Now.Year, 2, 15); // Assuming the current date is within the first month of the first quarter
    //        var hasSubmittedPreviousQuarterReturn = true;
    //        // Act
    //        var result = await _service.GetQuarterMonthsToDisplay(currentDate.Month, hasSubmittedPreviousQuarterReturn);
    //        // Assert
    //        Assert.AreEqual(2, result.QuarterlyMonths.Count);
    //        Assert.IsTrue(result.QuarterlyMonths.ContainsKey(1));           
    //        Assert.IsTrue(result.QuarterlyMonths.ContainsKey(2));
    //        Assert.AreEqual(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(currentDate.AddMonths(-1).Month), result.QuarterlyMonths[1]);           
    //        Assert.AreEqual(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(currentDate.Month), result.QuarterlyMonths[2]);
    //    }
    //    [TestMethod]
    //    public async Task GetQuarterMonthsToDisplay_WithinFirstMonthOfCurrentQuarterAndReturnSubmitted_ReturnsMonth_1_and_2_and_3_OfCurrentQuarter()
    //    {
    //        // Arrange
    //        var currentDate = new DateTime(DateTime.Now.Year, 3, 15); // Assuming the current date is within the first month of the first quarter
    //        var hasSubmittedPreviousQuarterReturn = true;
    //        // Act
    //        var result = await _service.GetQuarterMonthsToDisplay(currentDate.Month, hasSubmittedPreviousQuarterReturn);
    //        // Assert
    //        Assert.AreEqual(3, result.QuarterlyMonths.Count);
    //        Assert.IsTrue(result.QuarterlyMonths.ContainsKey(1));           
    //        Assert.IsTrue(result.QuarterlyMonths.ContainsKey(2));
    //        Assert.IsTrue(result.QuarterlyMonths.ContainsKey(3));     
    //        Assert.AreEqual(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(currentDate.AddMonths(-2).Month), result.QuarterlyMonths[1]);           
    //        Assert.AreEqual(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(currentDate.AddMonths(-1).Month), result.QuarterlyMonths[2]);           
    //        Assert.AreEqual(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(currentDate.Month), result.QuarterlyMonths[3]);
    //    }
    //    [TestMethod]
    //    public async Task GetQuarterMonthsToDisplay_WithinFirstMonthOfCurrentQuarter_April_AndReturnSubmitted_ReturnsFirstMonthOfCurrentQuarter()
    //    {
    //        // Arrange
    //        var currentDate = new DateTime(DateTime.Now.Year, 4, 15); // Assuming the current date is within the first month of the first quarter
    //        var hasSubmittedPreviousQuarterReturn = true;
    //        // Act
    //        var result = await _service.GetQuarterMonthsToDisplay(currentDate.Month, hasSubmittedPreviousQuarterReturn);
    //        // Assert
    //        Assert.AreEqual(1, result.QuarterlyMonths.Count);
    //        Assert.IsTrue(result.QuarterlyMonths.ContainsKey(4));
    //        Assert.AreEqual(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(currentDate.Month), result.QuarterlyMonths[4]);
    //    }
    //    [TestMethod]       
    //    public async Task GetQuarterMonthsToDisplay_WithinFirstMonthOfCurrentQuarterMayAndReturnSubmitted_ReturnsMonth_1_and_2_OfCurrentQuarter()
    //    {
    //        // Arrange
    //        var currentDate = new DateTime(DateTime.Now.Year, 5, 15); // Assuming the current date is within the first month of the first quarter
    //        var hasSubmittedPreviousQuarterReturn = true;
    //        // Act
    //        var result = await _service.GetQuarterMonthsToDisplay(currentDate.Month, hasSubmittedPreviousQuarterReturn);
    //        // Assert
    //        Assert.AreEqual(2, result.QuarterlyMonths.Count);
    //        Assert.IsTrue(result.QuarterlyMonths.ContainsKey(4));
    //        Assert.IsTrue(result.QuarterlyMonths.ContainsKey(5));
    //        Assert.AreEqual(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(currentDate.AddMonths(-1).Month), result.QuarterlyMonths[4]);
    //        Assert.AreEqual(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(currentDate.Month), result.QuarterlyMonths[5]);
    //    }
    //    [TestMethod]
    //    public async Task GetQuarterMonthsToDisplay_WithinFirstMonthOfCurrentQuarterJuneAndReturnSubmitted_ReturnsMonth_1_and_2_and_3_OfCurrentQuarter()
    //    {
    //        // Arrange
    //        var currentDate = new DateTime(DateTime.Now.Year, 6, 15); // Assuming the current date is within the first month of the first quarter
    //        var hasSubmittedPreviousQuarterReturn = true;
    //        // Act
    //        var result = await _service.GetQuarterMonthsToDisplay(currentDate.Month, hasSubmittedPreviousQuarterReturn);
    //        // Assert
    //        Assert.AreEqual(3, result.QuarterlyMonths.Count);
    //        Assert.IsTrue(result.QuarterlyMonths.ContainsKey(4));
    //        Assert.IsTrue(result.QuarterlyMonths.ContainsKey(5));
    //        Assert.IsTrue(result.QuarterlyMonths.ContainsKey(6));
    //        Assert.AreEqual(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(currentDate.AddMonths(-2).Month), result.QuarterlyMonths[4]);
    //        Assert.AreEqual(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(currentDate.AddMonths(-1).Month), result.QuarterlyMonths[5]);
    //        Assert.AreEqual(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(currentDate.Month), result.QuarterlyMonths[6]);
    //    }
    //    [TestMethod]
    //    public async Task GetQuarterMonthsToDisplay_WithinCurrentQuarter_ReturnsCorrectMonths()
    //    {
    //        // Arrange
    //        var currentDate = DateTime.Now.AddMonths(1); // Updated
    //        var hasSubmittedPreviousQuarterReturn = true;
    //        // Act
    //        var result = await _service.GetQuarterMonthsToDisplay(currentDate.Month, hasSubmittedPreviousQuarterReturn);
    //        // Assert
    //        Assert.AreEqual(2, result.QuarterlyMonths.Count);
    //        Assert.IsTrue(result.QuarterlyMonths.ContainsKey(1));
    //        Assert.IsTrue(result.QuarterlyMonths.ContainsKey(2));
    //    }
    //    [TestMethod]
    //    public async Task GetQuarterMonthsToDisplay_WithinReturnDeadlineAndPreviousQuarterReturnSubmitted_ReturnsMonth_1_and_2_OfCurrentQuarter()
    //    {
    //        // Arrange
    //        var currentDate = DateTime.Now.AddMonths(4); // Updated
    //        var hasSubmittedPreviousQuarterReturn = true;
    //        // Act
    //        var result = await _service.GetQuarterMonthsToDisplay(currentDate.Month, hasSubmittedPreviousQuarterReturn);
    //        // Assert
    //        Assert.AreEqual(2, result.QuarterlyMonths.Count);
    //        Assert.IsTrue(result.QuarterlyMonths.ContainsKey(4));
    //    }
    //    [TestMethod]
    //    public async Task GetQuarterMonthsToDisplay_WithinReturnDeadlineAndPreviousQuarterReturnNotSubmitted_ReturnsAllMonthsOfPreviousQuarter()
    //    {
    //        // Arrange
    //        var currentDate = new DateTime(DateTime.Now.Year, 4, 15);
    //        var hasSubmittedPreviousQuarterReturn = false;
    //        // Act
    //        var result = await _service.GetQuarterMonthsToDisplay(currentDate.Month, hasSubmittedPreviousQuarterReturn);
    //        // Assert
    //        Assert.AreEqual(3, result.QuarterlyMonths.Count);
    //        Assert.IsTrue(result.QuarterlyMonths.ContainsKey(1));
    //        Assert.IsTrue(result.QuarterlyMonths.ContainsKey(2));
    //        Assert.IsTrue(result.QuarterlyMonths.ContainsKey(3));
    //    }
    //    [TestMethod]
    //    public async Task GetQuarterMonthsToDisplay_AfterReturnDeadlineAndPreviousQuarterReturnSubmitted_ReturnsFirstMonthOfCurrentQuarter()
    //    {
    //        // Arrange
    //        var currentDate = DateTime.Now.AddMonths(5); // Updated
    //        var hasSubmittedPreviousQuarterReturn = true;
    //        // Act
    //        var result = await _service.GetQuarterMonthsToDisplay(currentDate.Month, hasSubmittedPreviousQuarterReturn);
    //        // Assert
    //        Assert.AreEqual(1, result.QuarterlyMonths.Count);
    //        Assert.IsTrue(result.QuarterlyMonths.ContainsKey(4));
    //    }
    //    [TestMethod]
    //    public async Task GetQuarterMonthsToDisplay_AfterReturnDeadlineAndPreviousQuarterReturnNotSubmitted_ReturnsFirstMonthOfCurrentQuarter()
    //    {
    //        // Arrange
    //        var currentDate = DateTime.Now.AddMonths(5); // Updated
    //        var hasSubmittedPreviousQuarterReturn = false;
    //        // Act
    //        var result = await _service.GetQuarterMonthsToDisplay(currentDate.Month, hasSubmittedPreviousQuarterReturn);
    //        // Assert
    //        Assert.AreEqual(1, result.QuarterlyMonths.Count);
    //        Assert.IsTrue(result.QuarterlyMonths.ContainsKey(4));
    //    }
    }
}
