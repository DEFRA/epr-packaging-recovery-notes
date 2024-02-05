using EPRN.Common.Enums;
using EPRN.Portal.ViewModels.PRNS;
using EPRN.Portal.ViewModels.Waste;

namespace EPRN.Portal.Services.Interfaces
{
    public interface IWasteService
    {
        Task<int> CreateJourney(
            int materialId,
            Category category);

        Task SaveSelectedWasteType(WasteTypeViewModel wasteTypesViewModel);

        Task<DuringWhichMonthRequestViewModel> GetQuarterForCurrentMonth(int journeyId);

        Task SaveSelectedMonth(DuringWhichMonthRequestViewModel viewModel);

        Task<RecordWasteViewModel> GetWasteTypesViewModel(int? id);

        Task<WasteSubTypesViewModel> GetWasteSubTypesViewModel(int journeyId);

        Task SaveSelectedWasteSubType(WasteSubTypesViewModel wasteSubTypesViewModel);

        Task<WhatHaveYouDoneWasteModel> GetWasteModel(int journeyId);

        Task<WasteRecordStatusViewModel> GetWasteRecordStatus(int journeyId);

        Task SaveWhatHaveYouDoneWaste(WhatHaveYouDoneWasteModel whatHaveYouDoneWasteModel);

        Task<ExportTonnageViewModel> GetExportTonnageViewModel(int journeyId);

        Task SaveTonnage(ExportTonnageViewModel exportTonnageViewModel);

        Task<BaledWithWireViewModel> GetBaledWithWireModel(int journeyId);

        Task SaveBaledWithWire(BaledWithWireViewModel baledWireModel);

        Task<ReProcessorExportViewModel> GetReProcessorExportViewModel(int journeyId);

        Task SaveReprocessorExport(ReProcessorExportViewModel reprocessorExportViewModel);

        Task<NoteViewModel> GetNoteViewModel(int journeyId);

        Task SaveNote(NoteViewModel noteViewModel);

        Task<string> GetWasteType(int journeyId);
    }
}
