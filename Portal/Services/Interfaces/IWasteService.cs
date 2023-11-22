using Portal.ViewModels;

namespace Portal.Services.Interfaces
{
    public interface IWasteService
    {
        DuringWhichMonthRequestViewModel GetCurrentQuarter(int journeyId);

        Task SaveSelectedMonth(int journeyId, int selectedMonth);

        Task<WasteTypesViewModel> GetWasteTypesViewModel(int journeyId);
    }
}
