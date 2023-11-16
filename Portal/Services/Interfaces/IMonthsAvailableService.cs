using Portal.Models;

namespace Portal.Services.Interfaces
{
    public interface IMonthsAvailableService
    {
        DuringWhichMonthRequestViewModel GetCurrentQuarter();
    }
}
