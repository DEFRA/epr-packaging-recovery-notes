
using AutoMapper;
using EPRN.Common.Enums;
using EPRN.Portal.Helpers.Interfaces;
using EPRN.Portal.Resources;
using EPRN.Portal.RESTServices.Interfaces;
using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels.Waste;

namespace EPRN.Portal.Services
{
    public class WasteService : IWasteService
    {
        private readonly IMapper _mapper;
        private readonly IHttpWasteService _httpWasteService;
        private readonly IHttpJourneyService _httpJourneyService;
        private readonly ILocalizationHelper<WhichQuarterResources> _localizationHelper;

        public WasteService(
            IMapper mapper,
            IHttpWasteService httpWasteService,
            IHttpJourneyService httpJourneyService,
            ILocalizationHelper<WhichQuarterResources> localizationHelper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _httpWasteService = httpWasteService ?? throw new ArgumentNullException(nameof(httpWasteService));
            _httpJourneyService = httpJourneyService ?? throw new ArgumentNullException(nameof(_httpJourneyService));
            _localizationHelper = localizationHelper ?? throw new ArgumentNullException(nameof(localizationHelper));
        }

        public async Task<int> CreateJourney(
            int materialId,
            Category category)
        {
            return await _httpJourneyService.CreateJourney(materialId, category);
        }

        public async Task<DuringWhichMonthRequestViewModel> GetQuarterForCurrentMonth(int journeyId, int currentMonth)
        {
            var category = await _httpJourneyService.GetCategory(journeyId);
            var whatHaveYouDoneWaste = await _httpJourneyService.GetWhatHaveYouDoneWaste(journeyId);

            var duringWhichMonthRequestViewModel = default(DuringWhichMonthRequestViewModel);

            if (whatHaveYouDoneWaste == DoneWaste.ReprocessedIt)
            {
                duringWhichMonthRequestViewModel = new DuringWhichMonthReceivedRequestViewModel();
            }
            else
            {
                duringWhichMonthRequestViewModel = new DuringWhichMonthSentOnRequestViewModel();
            }

            duringWhichMonthRequestViewModel.JourneyId = journeyId;
            duringWhichMonthRequestViewModel.WasteType = await _httpJourneyService.GetWasteType(journeyId);
            duringWhichMonthRequestViewModel.WhatHaveYouDone = whatHaveYouDoneWaste;
            duringWhichMonthRequestViewModel.Category = category;

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

            await _httpJourneyService.SaveSelectedMonth(
                duringWhichMonthRequestViewModel.JourneyId,
                duringWhichMonthRequestViewModel.SelectedMonth.Value);
        }

        public async Task<RecordWasteViewModel> GetWasteTypesViewModel()
        {
            return await Task.FromResult(
                new RecordWasteViewModel
                {
                    ExporterSiteMaterials = new ExporterSectionViewModel
                    {
                        Sites = new List<SiteSectionViewModel>
                        {
                            new SiteSectionViewModel
                            {
                                SiteName = "Tesco, Somewhere posh, England",
                                SiteMaterials = new Dictionary<int, string>
                                {
                                    { 4, "Steel" },
                                    { 12, "Paper" }
                                }
                            }
                        }
                    },
                    ReprocessorSiteMaterials = new ReproccesorSectionViewModel
                    {
                        Sites = new List<SiteSectionViewModel>
                        { 
                            new SiteSectionViewModel
                            {
                                SiteName = "Acme, London",
                                SiteMaterials = new Dictionary<int, string>
                                {
                                    { 2, "Cardboard" }
                                }
                            },
                            new SiteSectionViewModel
                            {
                                SiteName = "Microsoft, Swindon",
                                SiteMaterials = new Dictionary<int, string>
                                {
                                    { 4, "Steel" },
                                    { 2, "Cardboard" },
                                    { 15, "Glass" }
                                }
                            }
                        }
                    }
                });
        }

        public async Task<WasteSubTypesViewModel> GetWasteSubTypesViewModel(int journeyId)
        {
            var wasteTypeId = await _httpJourneyService.GetWasteTypeId(journeyId);

            if (wasteTypeId == null)
            {
                throw new ArgumentNullException($"Journey ID {journeyId} returned null Waste Type ID");
            }

            var selectedWasteSubTypeTask = _httpJourneyService.GetWasteSubTypeSelection(journeyId);
            var wasteMaterialSubTypesTask = _httpWasteService.GetWasteMaterialSubTypes(wasteTypeId.Value);

            await Task.WhenAll(
                selectedWasteSubTypeTask,
                wasteMaterialSubTypesTask);

            var wasteSubTypeOptions = _mapper.Map<List<WasteSubTypeOptionViewModel>>(wasteMaterialSubTypesTask.Result);

            return new WasteSubTypesViewModel
            {
                JourneyId = journeyId,
                WasteSubTypeOptions = wasteSubTypeOptions,
                SelectedWasteSubTypeId = selectedWasteSubTypeTask.Result.WasteSubTypeId,
                CustomPercentage = selectedWasteSubTypeTask.Result.Adjustment
            };
        }

        public async Task SaveSelectedWasteSubType(WasteSubTypesViewModel wasteSubTypesViewModel)
        {
            if (wasteSubTypesViewModel == null)
                throw new ArgumentNullException(nameof(wasteSubTypesViewModel));

            if (wasteSubTypesViewModel.SelectedWasteSubTypeId == null)
                throw new ArgumentNullException(nameof(wasteSubTypesViewModel.SelectedWasteSubTypeId));

            if (wasteSubTypesViewModel.AdjustmentRequired && wasteSubTypesViewModel.CustomPercentage == null)
                throw new ArgumentNullException(nameof(wasteSubTypesViewModel.CustomPercentage));

            (int wasteSubTypeId, double adjustment) = ProcessSubTypePayload(wasteSubTypesViewModel);

            await _httpJourneyService.SaveSelectedWasteSubType(
                wasteSubTypesViewModel.JourneyId,
                wasteSubTypeId, adjustment);
        }

        private (int wasteSubTypeId, double adjustment) ProcessSubTypePayload(WasteSubTypesViewModel wasteSubTypesViewModel)
        {
            int wasteSubTypeId = wasteSubTypesViewModel.SelectedWasteSubTypeId.Value;
            double adjustment = 0;

            if (wasteSubTypesViewModel.AdjustmentRequired)
            {
                adjustment = wasteSubTypesViewModel.CustomPercentage.Value;
            }
            else
            {
                adjustment = wasteSubTypesViewModel.WasteSubTypeOptions.FirstOrDefault(subTypeOption => subTypeOption.Id == wasteSubTypeId).Adjustment.Value;
            }

            return (wasteSubTypeId, adjustment);
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

        //public async Task SaveSelectedMonth(WasteTypesViewModel wasteTypesViewModel)
        //{
        //    if (wasteTypesViewModel == null)
        //        throw new ArgumentNullException(nameof(wasteTypesViewModel));

        //    if (wasteTypesViewModel.SelectedWasteTypeId == null)
        //        throw new ArgumentNullException(nameof(wasteTypesViewModel.SelectedWasteTypeId));

        //    await _httpJourneyService.SaveSelectedWasteType(
        //        wasteTypesViewModel.JourneyId,
        //        wasteTypesViewModel.SelectedWasteTypeId.Value);
        //}

        public async Task SaveWhatHaveYouDoneWaste(WhatHaveYouDoneWasteModel whatHaveYouDoneWasteViewModel)
        {
            if (whatHaveYouDoneWasteViewModel == null)
                throw new ArgumentNullException(nameof(whatHaveYouDoneWasteViewModel));

            if (whatHaveYouDoneWasteViewModel.WhatHaveYouDone == null)
                throw new ArgumentNullException(nameof(whatHaveYouDoneWasteViewModel.WhatHaveYouDone));

            await _httpJourneyService.SaveWhatHaveYouDoneWaste(whatHaveYouDoneWasteViewModel.JourneyId, whatHaveYouDoneWasteViewModel.WhatHaveYouDone.Value);
        }

        public async Task<WasteRecordStatusViewModel> GetWasteRecordStatus(int journeyId)
        {
            var result = await _httpJourneyService.GetWasteRecordStatus(journeyId);
            return _mapper.Map<WasteRecordStatusViewModel>(result);
        }

        public async Task<ExportTonnageViewModel> GetExportTonnageViewModel(int journeyId)
        {
            return new ExportTonnageViewModel
            {
                JourneyId = journeyId,
                ExportTonnes = await _httpJourneyService.GetWasteTonnage(journeyId)
            };
        }

        public async Task SaveTonnage(ExportTonnageViewModel exportTonnageViewModel)
        {
            if (exportTonnageViewModel == null)
                throw new ArgumentNullException(nameof(exportTonnageViewModel));

            if (exportTonnageViewModel.ExportTonnes == null)
                throw new ArgumentNullException(nameof(exportTonnageViewModel.ExportTonnes));

            await _httpJourneyService.SaveTonnage(
                exportTonnageViewModel.JourneyId,
                exportTonnageViewModel.ExportTonnes.Value);
        }


        public async Task<BaledWithWireViewModel> GetBaledWithWireModel(int journeyId)
        {
            var dto = await _httpJourneyService.GetBaledWithWire(journeyId);
            var vm = _mapper.Map<BaledWithWireViewModel>(dto);

            return vm;
        }

        public async Task SaveBaledWithWire(BaledWithWireViewModel baledWireModel)
        {
            if (baledWireModel == null)
                throw new ArgumentNullException(nameof(baledWireModel));

            if (baledWireModel.BaledWithWire == null)
                throw new ArgumentNullException(nameof(baledWireModel.BaledWithWire));

            if (baledWireModel.BaledWithWireDeductionPercentage == null)
                throw new ArgumentNullException(nameof(baledWireModel.BaledWithWireDeductionPercentage));


            await _httpJourneyService.SaveBaledWithWire(
                baledWireModel.JourneyId, 
                baledWireModel.BaledWithWire.Value, 
                baledWireModel.BaledWithWire.Value == true ? baledWireModel.BaledWithWireDeductionPercentage.Value : 0);
        }

        public async Task<ReProcessorExportViewModel> GetReProcessorExportViewModel(int journeyId)
        {
            var reProcessorExport = new ReProcessorExportViewModel()
            {
                JourneyId = journeyId,
            };

            return reProcessorExport;
        }

        public async Task SaveReprocessorExport(ReProcessorExportViewModel reProcessorExportViewModel)
        {
            if (reProcessorExportViewModel == null)
                throw new ArgumentNullException(nameof(reProcessorExportViewModel));

            if (reProcessorExportViewModel == null)
                throw new ArgumentNullException(nameof(reProcessorExportViewModel.JourneyId));

            if (reProcessorExportViewModel.SelectedSite == null)
                throw new ArgumentNullException(nameof(reProcessorExportViewModel.SelectedSite));

            await _httpJourneyService.SaveReprocessorExport(reProcessorExportViewModel.JourneyId, reProcessorExportViewModel.SelectedSite.Value);
        }

        public async Task<NoteViewModel> GetNoteViewModel(int journeyId)
        {
            var noteViewModel = new NoteViewModel
            {
                JourneyId = journeyId,
                WasteType = await _httpJourneyService.GetNote(journeyId)
            };

            return noteViewModel;
        }

        public async Task SaveNote(NoteViewModel noteViewModel)
        {
            if (noteViewModel == null)
                throw new ArgumentNullException(nameof(noteViewModel));

            if (noteViewModel.NoteContent == null)
                throw new ArgumentNullException(nameof(noteViewModel.NoteContent));

            await _httpJourneyService.SaveNote(
                noteViewModel.JourneyId,
                noteViewModel.NoteContent);
        }
    }
}
