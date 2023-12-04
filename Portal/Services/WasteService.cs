using AutoMapper;
using EPRN.Portal.Helpers.Interfaces;
using EPRN.Portal.Resources;
using EPRN.Portal.RESTServices.Interfaces;
using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels;

namespace EPRN.Portal.Services
{
    public class WasteService : IWasteService
    {
        private readonly IMapper _mapper;
        private readonly IHttpWasteService _httpWasteService;
        private readonly ILocalizationHelper<WhichQuarterResources> _localizationHelper;

        public WasteService(
            IMapper mapper,
            IHttpWasteService httpWasteService,
            ILocalizationHelper<WhichQuarterResources> localizationHelper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _httpWasteService = httpWasteService ?? throw new ArgumentNullException(nameof(httpWasteService));
            _localizationHelper = localizationHelper ?? throw new ArgumentNullException(nameof(localizationHelper));
        }

        public async Task<DuringWhichMonthRequestViewModel> GetQuarterForCurrentMonth(int journeyId, int currentMonth)
        {
            var duringWhichMonthRequestViewModel = new DuringWhichMonthRequestViewModel
            {
                JourneyId = journeyId,
                // We're not part of a journey yet, so this can't really be hooked up
                WasteType = await _httpWasteService.GetWasteType(journeyId)
            };

            int firstMonthOfQuarter = (currentMonth - 1) / 3 * 3 + 1;
            duringWhichMonthRequestViewModel.Quarter.Add(firstMonthOfQuarter, _localizationHelper.GetString($"Month{firstMonthOfQuarter}"));
            duringWhichMonthRequestViewModel.Quarter.Add(firstMonthOfQuarter + 1, _localizationHelper.GetString($"Month{firstMonthOfQuarter + 1}"));
            duringWhichMonthRequestViewModel.Quarter.Add(firstMonthOfQuarter + 2, _localizationHelper.GetString($"Month{firstMonthOfQuarter + 2}"));

            return duringWhichMonthRequestViewModel;
        }

        public async Task SaveSelectedMonth(DuringWhichMonthRequestViewModel duringWhichMonthRequestViewModel)
        {
            if (duringWhichMonthRequestViewModel == null)
                throw new ArgumentNullException(nameof(duringWhichMonthRequestViewModel));

            if (duringWhichMonthRequestViewModel.SelectedMonth == null)
                throw new ArgumentNullException(nameof(duringWhichMonthRequestViewModel.SelectedMonth));

            await _httpWasteService.SaveSelectedMonth(
                duringWhichMonthRequestViewModel.JourneyId,
                duringWhichMonthRequestViewModel.SelectedMonth.Value);
        }

        public async Task<WasteTypesViewModel> GetWasteTypesViewModel(int journeyId)
        {
            var wasteMaterialTypes = await _httpWasteService.GetWasteMaterialTypes();

            return new WasteTypesViewModel
            {
                JourneyId = journeyId,
                WasteTypes = wasteMaterialTypes.ToDictionary(t => t.Id, t => t.Name)
            };
        }

        public async Task SaveSelectedWasteType(WasteTypesViewModel wasteTypesViewModel)
        {
            if (wasteTypesViewModel == null)
                throw new ArgumentNullException(nameof(wasteTypesViewModel));

            if (wasteTypesViewModel.SelectedWasteTypeId == null)
                throw new ArgumentNullException(nameof(wasteTypesViewModel.SelectedWasteTypeId));

            await _httpWasteService.SaveSelectedWasteType(
                wasteTypesViewModel.JourneyId,
                wasteTypesViewModel.SelectedWasteTypeId.Value);
        }

        public async Task<WhatHaveYouDoneWasteModel> GetWasteModel(int journeyId)
        {
            await Task.CompletedTask;

            var whatHaveYouDoneWasteModel = new WhatHaveYouDoneWasteModel()
            {
                JourneyId = journeyId,
                // We're not part of a journey yet, so this can't really be hooked up
                //WasteType = await _httpWasteService.GetWasteType(journeyId)
            };

            return whatHaveYouDoneWasteModel;
        }

        public async Task SaveSelectedMonth(WasteTypesViewModel wasteTypesViewModel)
        {
            if (wasteTypesViewModel == null)
                throw new ArgumentNullException(nameof(wasteTypesViewModel));

            if (wasteTypesViewModel.SelectedWasteTypeId == null)
                throw new ArgumentNullException(nameof(wasteTypesViewModel.SelectedWasteTypeId));

            await _httpWasteService.SaveSelectedWasteType(
                wasteTypesViewModel.JourneyId,
                wasteTypesViewModel.SelectedWasteTypeId.Value);
        }

        public async Task SaveWhatHaveYouDoneWaste(WhatHaveYouDoneWasteModel whatHaveYouDoneWasteViewModel)
        {
            if (whatHaveYouDoneWasteViewModel == null)
                throw new ArgumentNullException(nameof(whatHaveYouDoneWasteViewModel));

            if (whatHaveYouDoneWasteViewModel.WhatHaveYouDone == null)
                throw new ArgumentNullException(nameof(whatHaveYouDoneWasteViewModel.WhatHaveYouDone));

            await _httpWasteService.SaveWhatHaveYouDoneWaste(whatHaveYouDoneWasteViewModel.JourneyId, whatHaveYouDoneWasteViewModel.WhatHaveYouDone.Value);
        }

        public async Task<WasteRecordStatusViewModel> GetWasteRecordStatus(int journeyId)
        {
            var result = await _httpWasteService.GetWasteRecordStatus(journeyId);
            return _mapper.Map<WasteRecordStatusViewModel>(result);
        }

        public ExportTonnageViewModel GetExportTonnageViewModel(int journeyId)
        {
            return new ExportTonnageViewModel
            {
                JourneyId = journeyId
            };
        }

        public async Task SaveTonnage(ExportTonnageViewModel exportTonnageViewModel)
        {
            if (exportTonnageViewModel == null)
                throw new ArgumentNullException(nameof(exportTonnageViewModel));

            if (exportTonnageViewModel.ExportTonnes == null)
                throw new ArgumentNullException(nameof(exportTonnageViewModel.ExportTonnes));

            await _httpWasteService.SaveTonnage(
                exportTonnageViewModel.JourneyId, 
                exportTonnageViewModel.ExportTonnes.Value);
        }
    }
}
