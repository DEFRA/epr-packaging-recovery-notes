using Portal.ViewModels;

namespace Portal.Services.Interfaces
{
    public interface IWasteService
    {
        Task<DuringWhichMonthRequestViewModel> GetCurrentQuarter(int journeyId);

        Task SaveSelectedMonth(DuringWhichMonthRequestViewModel duringWhichMonthRequestViewModel);

        Task<WasteTypesViewModel> GetWasteTypesViewModel(int journeyId);

        Task SaveSelectedWasteType(WasteTypesViewModel wasteTypesViewModel);

        Task<WhatHaveYouDoneWasteModel> GetWasteModel(int journeyId);

        Task SaveSelectedWasteType(int journeyId, string selectedWasteType);

        Task<WasteRecordStatusViewModel> GetWasteRecordStatus(int journeyId);
    }
}
