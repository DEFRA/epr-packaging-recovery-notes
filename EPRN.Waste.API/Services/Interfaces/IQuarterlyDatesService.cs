using EPRN.Common.Dtos;

namespace EPRN.Waste.API.Services.Interfaces
{
    public interface IQuarterlyDatesService
    {
        Task<QuarterlyDatesDto> GetQuarterMonthsToDisplay(int currentMonth, bool hasSubmittedPreviousQuarterReturn);
    }
}
