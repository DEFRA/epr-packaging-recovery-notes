﻿using System.Globalization;
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

        public async Task<DuringWhichMonthRequestViewModel> GetQuarterForCurrentMonth(int Id)
        {
            var category = await _httpJourneyService.GetCategory(Id);
            var whatHaveYouDoneWaste = await _httpJourneyService.GetWhatHaveYouDoneWaste(Id);
            var duringWhichMonthRequestViewModel = CreateDuringWhichMonthRequestViewModel(whatHaveYouDoneWaste);
            await PopulateViewModel(duringWhichMonthRequestViewModel, Id, whatHaveYouDoneWaste);
            return duringWhichMonthRequestViewModel;
        }
        
        private static DuringWhichMonthRequestViewModel CreateDuringWhichMonthRequestViewModel(DoneWaste whatHaveYouDoneWaste)
        {
            return whatHaveYouDoneWaste == DoneWaste.ReprocessedIt
                ? new DuringWhichMonthReceivedRequestViewModel()
                : new DuringWhichMonthSentOnRequestViewModel();
        }

        private async Task PopulateViewModel(DuringWhichMonthRequestViewModel viewModel, int Id, DoneWaste whatHaveYouDoneWaste)
        {
            // TODO -- Add has submitted qtr return this logic when available, true for now
            const bool hasSubmittedPreviousQuarterReturn = true;
            
            viewModel.Id = Id;
            viewModel.WhatHaveYouDone = whatHaveYouDoneWaste;
            
            var quarterDates = await _httpJourneyService.GetQuarterlyMonths(Id, DateTime.Now.Month, hasSubmittedPreviousQuarterReturn);

            viewModel.Notification = quarterDates.Notification;
            viewModel.SubmissionDate = quarterDates.SubmissionDate;
            viewModel.NotificationDeadlineDate = quarterDates.NotificationDeadlineDate.ToString("d MMMM", CultureInfo.InvariantCulture);
            viewModel.Category = whatHaveYouDoneWaste == DoneWaste.ReprocessedIt
                ? Category.Reprocessor
                : Category.Exporter;

            var rm = new ResourceManager("EPRN.Portal.Resources.WhichQuarterResources",
                Assembly.GetExecutingAssembly());

            foreach (var itemMonth in quarterDates.QuarterlyMonths)
            {
                var (value, suffix) = ProcessValue(itemMonth.Value);
                var resourceString = GetResourceString(value, suffix, rm);
                viewModel.Quarter.Add(itemMonth.Key, resourceString);
            }
        }

        /// <summary>
        /// Processes the input string and separates it into a value and a year suffix.
        /// The suffix is the last five characters of the input string if they start with a space.
        /// </summary>
        /// <param name="value">The input string to be processed.</param>
        /// <returns>A tuple containing the processed value and suffix.</returns>
        private static (string value, string suffix) ProcessValue(string value)
        {
            var suffix = "";
            if (value.Length > 5 && value[^5] == ' ')
            {
                suffix = value[^5..];
                value = value[..^5];
            }
            return (value, suffix);
        }
        /// <summary>
        /// Retrieves a resource string from the resource manager and appends a year suffix to it.
        /// </summary>
        /// <param name="value">The name of the resource string to retrieve.</param>
        /// <param name="suffix">The suffix to append to the resource string.</param>
        /// <param name="rm">The resource manager to retrieve the resource string from.</param>
        /// <returns>The resource string with the suffix appended.</returns>
        private static string GetResourceString(string value, string suffix, ResourceManager rm)
        {
            return rm.GetString(value) + suffix;
        }


        public async Task SaveSelectedMonth(DuringWhichMonthRequestViewModel duringWhichMonthRequestViewModel)
        {
            if (duringWhichMonthRequestViewModel == null)
                throw new ArgumentNullException(nameof(duringWhichMonthRequestViewModel));

            if (duringWhichMonthRequestViewModel.SelectedMonth == null)
                throw new ArgumentNullException(nameof(duringWhichMonthRequestViewModel.SelectedMonth));

            await _httpJourneyService.SaveSelectedMonth(
                duringWhichMonthRequestViewModel.Id,
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
                        new() {
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
                        new() {
                            SiteName = "Lansbourne Trading Estate, Edinburgh",
                            SiteMaterials = new Dictionary<int, string>
                            {
                                { 2, materialTypes[2] }
                            }
                        },
                        new() {
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
        
        public async Task<WasteSubTypesViewModel> GetWasteSubTypesViewModel(int Id)
        {
            var wasteTypeId = await _httpJourneyService.GetWasteTypeId(Id);

            if (wasteTypeId == null)
            {
                throw new ArgumentNullException($"Journey ID {Id} returned null Waste Type ID");
            }

            var selectedWasteSubTypeTask = _httpJourneyService.GetWasteSubTypeSelection(Id);
            var wasteMaterialSubTypesTask = _httpWasteService.GetWasteMaterialSubTypes(wasteTypeId.Value);

            await Task.WhenAll(
                selectedWasteSubTypeTask,
                wasteMaterialSubTypesTask);

            var wasteSubTypeOptions = _mapper.Map<List<WasteSubTypeOptionViewModel>>(wasteMaterialSubTypesTask.Result);

            return new WasteSubTypesViewModel
            {
                Id = Id,
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
                wasteSubTypesViewModel.Id,
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

        public async Task<WhatHaveYouDoneWasteModel> GetWasteModel(int Id)
        {
            await Task.CompletedTask;

            var whatHaveYouDoneWasteModel = new WhatHaveYouDoneWasteModel
            {
                Id = Id
            };

            return whatHaveYouDoneWasteModel;
        }

        public async Task SaveWhatHaveYouDoneWaste(WhatHaveYouDoneWasteModel whatHaveYouDoneWasteViewModel)
        {
            if (whatHaveYouDoneWasteViewModel == null)
                throw new ArgumentNullException(nameof(whatHaveYouDoneWasteViewModel));

            if (whatHaveYouDoneWasteViewModel.WhatHaveYouDone == null)
                throw new ArgumentNullException(nameof(whatHaveYouDoneWasteViewModel.WhatHaveYouDone));

            await _httpJourneyService.SaveWhatHaveYouDoneWaste(whatHaveYouDoneWasteViewModel.Id, whatHaveYouDoneWasteViewModel.WhatHaveYouDone.Value);
        }

        public async Task<WasteRecordStatusViewModel> GetWasteRecordStatus(int Id)
        {
            var result = await _httpJourneyService.GetWasteRecordStatus(Id);
            return _mapper.Map<WasteRecordStatusViewModel>(result);
        }

        public async Task<ExportTonnageViewModel> GetExportTonnageViewModel(int Id) => new ExportTonnageViewModel
        {
            Id = Id,
            ExportTonnes = await _httpJourneyService.GetWasteTonnage(Id)
        };

        public async Task SaveTonnage(ExportTonnageViewModel exportTonnageViewModel)
        {
            if (exportTonnageViewModel == null)
                throw new ArgumentNullException(nameof(exportTonnageViewModel));

            if (exportTonnageViewModel.ExportTonnes == null)
                throw new ArgumentNullException(nameof(exportTonnageViewModel.ExportTonnes));

            await _httpJourneyService.SaveTonnage(
                exportTonnageViewModel.Id,
                exportTonnageViewModel.ExportTonnes.Value);
        }


        public async Task<BaledWithWireViewModel> GetBaledWithWireModel(int Id)
        {
            var dto = await _httpJourneyService.GetBaledWithWire(Id);
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
                baledWireModel.Id, 
                baledWireModel.BaledWithWire.Value, 
                baledWireModel.BaledWithWire.Value == true ? baledWireModel.BaledWithWireDeductionPercentage.Value : 0);
        }

        public async Task<ReProcessorExportViewModel> GetReProcessorExportViewModel(int Id)
        {
            var reProcessorExport = new ReProcessorExportViewModel()
            {
                Id = Id,
            };

            return reProcessorExport;
        }

        public async Task SaveReprocessorExport(ReProcessorExportViewModel reProcessorExportViewModel)
        {
            if (reProcessorExportViewModel == null)
                throw new ArgumentNullException(nameof(reProcessorExportViewModel));

            if (reProcessorExportViewModel.SelectedSite == null)
                throw new ArgumentNullException(nameof(reProcessorExportViewModel.SelectedSite));

            await _httpJourneyService.SaveReprocessorExport(reProcessorExportViewModel.Id, reProcessorExportViewModel.SelectedSite.Value);
        }

        public async Task<NoteViewModel> GetNoteViewModel(int Id)
        {
            var noteDto = await _httpJourneyService.GetNote(journeyId);
            if (noteDto == null)
                throw new ArgumentNullException(nameof(NoteViewModel));

            var noteViewModel = new NoteViewModel
            {
                Id = Id,
                NoteContent = noteDto.Note,
                WasteCategory = noteDto.WasteCategory
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
                noteViewModel.Id,
                noteViewModel.NoteContent);
        }

        public async Task<string> GetWasteType(int Id)
        {
            return await _httpJourneyService.GetWasteType(Id);
        }
    }
}
