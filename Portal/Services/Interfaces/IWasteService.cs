using Portal.ViewModels;

namespace Portal.Services.Interfaces
{
    public interface IWasteService
    {
        DuringWhichMonthRequestViewModel GetCurrentQuarter(int journeyId);

        Task SaveSelectedMonth(DuringWhichMonthRequestViewModel duringWhichMonthRequestViewModel);

        Task<WasteTypesViewModel> GetWasteTypesViewModel(int journeyId);

        Task SaveSelectedWasteType(WasteTypesViewModel wasteTypesViewModel);

        //Task<string> GetWasteType(int journeyId);
    }
}
