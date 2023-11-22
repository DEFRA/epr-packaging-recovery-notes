using Portal.ViewModels;

namespace Portal.Services.Interfaces
{
    public interface IWasteService
    {
        Task SaveSelectedMonth(int journeyId, int selectedMonth);
        DuringWhichMonthRequestViewModel GetCurrentQuarter(int journeyId);
        Task<WasteRecordStatusViewModel> GetWasteRecordStatus(int reprocessorId);
    }
}
