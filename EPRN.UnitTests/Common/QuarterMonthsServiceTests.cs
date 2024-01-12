using EPRN.Common.Services;

namespace EPRN.UnitTests.Common;

[TestClass]
public class QuarterMonthsServiceTests
{

    [TestMethod]
    public void first_month_current_quarter__return_month_1()
    {
        var result = QuarterMonthDisplay.GetQuarterMonths(DateTime.Today, false);
        Assert.AreEqual(result.Count, 1);
    }

    [TestMethod]
    public void second_month_current_quarter__return_month_1_and_2()
    {
        var result = QuarterMonthDisplay.GetQuarterMonths(DateTime.Today.AddMonths(1), false);
        Assert.AreEqual(result.Count, 2);
    }

    [TestMethod]
    public void third_month_current_quarter__return_month_1_and_2_and_3()
    {
        var result = QuarterMonthDisplay.GetQuarterMonths(DateTime.Today.AddMonths(2), false);
        Assert.AreEqual(result.Count, 3);
    }
    
    [TestMethod]
    public void third_month_next_quarter__return_month_4()
    {
        var result = QuarterMonthDisplay.GetQuarterMonths(DateTime.Today.AddMonths(2), false);
        Assert.AreEqual(result.Count, 4);
    }
    
    
}