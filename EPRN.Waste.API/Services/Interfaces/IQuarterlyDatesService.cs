namespace EPRN.Waste.API.Services.Interfaces
{
    public interface IQuarterlyDatesService
    {
        Task<Dictionary<int, string>> GetQuarterMonthsToDisplay(int currentMonth, bool hasSubmittedPreviousQuarterReturn);
    }
}
