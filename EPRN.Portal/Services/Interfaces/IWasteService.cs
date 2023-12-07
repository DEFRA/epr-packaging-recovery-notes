using EPRN.Common.Enums;
using EPRN.Portal.ViewModels;

namespace EPRN.Portal.Services.Interfaces
{
    public interface IWasteService
    {
        Task<DuringWhichMonthRequestViewModel> GetQuarterForCurrentMonth(int journeyId, int currentMonth, DoneWaste doneWaste);

        Task SaveSelectedMonth(DuringWhichMonthRequestViewModel duringWhichMonthRequestViewModel);

        Task<WasteTypesViewModel> GetWasteTypesViewModel(int journeyId);

        Task SaveSelectedWasteType(WasteTypesViewModel wasteTypesViewModel);

        Task<WhatHaveYouDoneWasteModel> GetWasteModel(int journeyId);

        Task<WasteRecordStatusViewModel> GetWasteRecordStatus(int journeyId);

        Task SaveWhatHaveYouDoneWaste(WhatHaveYouDoneWasteModel whatHaveYouDoneWasteModel);

        ExportTonnageViewModel GetExportTonnageViewModel(int journeyId);

        Task SaveTonnage(ExportTonnageViewModel exportTonnageViewModel);
    }
}
