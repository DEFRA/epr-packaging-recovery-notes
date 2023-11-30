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

        Task<WasteRecordStatusViewModel> GetWasteRecordStatus(int journeyId);
        Task SaveWhatHaveYouDoneWaste(WhatHaveYouDoneWasteModel whatHaveYouDoneWasteModel);

        Task<BaledWithWireModel> GetBaledWithWireModel(int journeyId);

        Task SaveBaledWithWire(BaledWithWireModel baledWireModel);
    }
}
