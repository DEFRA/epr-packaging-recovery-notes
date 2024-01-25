using System.Globalization;
using System.Reflection;
using System.Resources;
using AutoMapper;
using EPRN.Common.Enums;
using EPRN.Portal.Configuration;
using EPRN.Portal.Helpers.Interfaces;
using EPRN.Portal.Resources;
using EPRN.Portal.RESTServices.Interfaces;
using EPRN.Portal.Services.Interfaces;
using EPRN.Portal.ViewModels.Waste;
using Microsoft.Extensions.Options;

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
            ILocalizationHelper<WhichQuarterResources> localizationHelper,
            IOptions<AppConfigSettings> configSettings)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _httpWasteService = httpWasteService ?? throw new ArgumentNullException(nameof(httpWasteService));
            _httpJourneyService = httpJourneyService ?? throw new ArgumentNullException(nameof(httpJourneyService));
            _localizationHelper = localizationHelper ?? throw new ArgumentNullException(nameof(localizationHelper));
        }

        public async Task<int> CreateJourney(
            int materialId,
            Category category)
        {
            return await _httpJourneyService.CreateJourney(materialId, category);
        }

        public async Task SaveSelectedWasteType(WasteTypeViewModel wasteTypesViewModel)
        {
            if (wasteTypesViewModel == null)
                throw new ArgumentNullException(nameof(wasteTypesViewModel));

            await _httpJourneyService.SaveSelectedWasteType(
                wasteTypesViewModel.Id,
                wasteTypesViewModel.MaterialId);
        }

        public async Task<DuringWhichMonthRequestViewModel> GetQuarterForCurrentMonth(int journeyId)
        {
            var category = await _httpJourneyService.GetCategory(journeyId);
            var whatHaveYouDoneWaste = await _httpJourneyService.GetWhatHaveYouDoneWaste(journeyId);
            var duringWhichMonthRequestViewModel = CreateDuringWhichMonthRequestViewModel(whatHaveYouDoneWaste);
            await PopulateViewModel(duringWhichMonthRequestViewModel, journeyId, whatHaveYouDoneWaste);
            return duringWhichMonthRequestViewModel;
        }
        
        private static DuringWhichMonthRequestViewModel CreateDuringWhichMonthRequestViewModel(DoneWaste whatHaveYouDoneWaste)
        {
            return whatHaveYouDoneWaste == DoneWaste.ReprocessedIt
                ? new DuringWhichMonthReceivedRequestViewModel()
                : new DuringWhichMonthSentOnRequestViewModel();
        }

        private async Task PopulateViewModel(DuringWhichMonthRequestViewModel viewModel, int journeyId, DoneWaste whatHaveYouDoneWaste)
        {
            // TODO -- Add has submitted qtr return this logic when available, true for now
            const bool hasSubmittedPreviousQuarterReturn = true;
            
            viewModel.JourneyId = journeyId;
            viewModel.WhatHaveYouDone = whatHaveYouDoneWaste;
            
//            var wasteTypeTask = _httpJourneyService.GetWasteType(journeyId);
            var quarterDates = await _httpJourneyService.GetQuarterlyMonths(journeyId, DateTime.Now.Month, hasSubmittedPreviousQuarterReturn);

//            await Task.WhenAll(wasteTypeTask, quarterTask);
//            viewModel.WasteType = wasteTypeTask.Result;
            viewModel.Notification = quarterDates.Notification;
            viewModel.SubmissionDate = quarterDates.SubmissionDate;
            viewModel.NotificationDeadlineDate = quarterDates.NotificationDeadlineDate.ToString("d MMMM", CultureInfo.InvariantCulture);

            var rm = new ResourceManager("EPRN.Portal.Resources.WhichQuarterResources",
                Assembly.GetExecutingAssembly());
            
            foreach (var itemMonth in quarterDates.QuarterlyMonths)
            {
                var value = itemMonth.Value;
                var suffix = "";
                
                // Check if the value has a suffix of underscore then year
                if (value.Length > 5 && value[^5] == ' ')
                {
                    // Remove the year suffix and save it in the suffix variable
                    suffix = value[^5..];
                    value = value[..^5];
                }
                
                // Get the string from the resource manager and add the suffix back
                var resourceString = rm.GetString(value) + suffix;
                viewModel.Quarter.Add(itemMonth.Key, resourceString);
            }
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

        public async Task<RecordWasteViewModel> GetWasteTypesViewModel(int? id)
        {
            var category = Category.Unknown;

            if (id.HasValue)
                category = await _httpJourneyService.GetCategory(id.Value);
            // get waste types from the API
            var materialTypes = await _httpWasteService.GetWasteMaterialTypes();

            var viewModel = new RecordWasteViewModel
            {
                Id = id,
                Category = category,
                ExporterSiteMaterials = new ExporterSectionViewModel
                {
                    Sites = new List<SiteSectionViewModel>
                    {
                        new SiteSectionViewModel
                        {
                            SiteName = "123 Letsbe Avenue, Policeville",
                            SiteMaterials = new Dictionary<int, string>
                            {
                                { 4, materialTypes[4] },
                                { 9, materialTypes[9] }
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
                            SiteName = "Lansbourne Trading Estate, Edinburgh",
                            SiteMaterials = new Dictionary<int, string>
                            {
                                { 2, materialTypes[2] }
                            }
                        },
                        new SiteSectionViewModel
                        {
                            SiteName = "1 Banburgh Drive, Cardiff",
                            SiteMaterials = new Dictionary<int, string>
                            {
                                { 4, materialTypes[4] },
                                { 2, materialTypes[2] },
                                { 7, materialTypes[7] }
                            }
                        }
                    }
                }
            };

            return viewModel;
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
                NoteContent = await _httpJourneyService.GetNote(journeyId)
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

        public async Task<string> GetWasteType(int journeyId)
        {
            return await _httpJourneyService.GetWasteType(journeyId);
        }
    }
}
