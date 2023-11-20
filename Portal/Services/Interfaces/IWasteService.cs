using Portal.Models;

namespace Portal.Services.Interfaces
{
    public interface IWasteService
    {
        DuringWhichMonthRequestViewModel GetCurrentQuarter(int journeyId);
    }
}
