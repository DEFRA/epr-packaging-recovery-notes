﻿using AutoMapper;
using Portal.Resources;
using Portal.RESTServices.Interfaces;
using Portal.Services.Interfaces;
using Portal.ViewModels;

namespace Portal.Services
{
    public class WasteService : IWasteService
    {
        private readonly IMapper _mapper;
        private readonly IHttpWasteService _httpWasteService;

        public WasteService(
            IMapper mapper,
            IHttpWasteService httpWasteService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _httpWasteService = httpWasteService ?? throw new ArgumentNullException(nameof(httpWasteService));
        }

        public async Task<DuringWhichMonthRequestViewModel> GetCurrentQuarter(int journeyId)
        {
            var duringWhichMonthRequestViewModel = new DuringWhichMonthRequestViewModel
            {
                JourneyId = journeyId,
                WasteType = await _httpWasteService.GetWasteType(journeyId)
            };

            int currentMonth = DateTime.Now.Month;

            switch (currentMonth)
            {
                case 1:
                case 2:
                case 3:
                    duringWhichMonthRequestViewModel.Quarter.Add(1, @WhichQuarterResources.January);
                    duringWhichMonthRequestViewModel.Quarter.Add(2, @WhichQuarterResources.February);
                    duringWhichMonthRequestViewModel.Quarter.Add(3, @WhichQuarterResources.March);
                    break;

                case 4:
                case 5:
                case 6:
                    duringWhichMonthRequestViewModel.Quarter.Add(4, @WhichQuarterResources.April);
                    duringWhichMonthRequestViewModel.Quarter.Add(5, @WhichQuarterResources.May);
                    duringWhichMonthRequestViewModel.Quarter.Add(6, @WhichQuarterResources.June);
                    break;
                case 7:
                case 8:
                case 9:
                    duringWhichMonthRequestViewModel.Quarter.Add(7, @WhichQuarterResources.July);
                    duringWhichMonthRequestViewModel.Quarter.Add(8, @WhichQuarterResources.August);
                    duringWhichMonthRequestViewModel.Quarter.Add(9, @WhichQuarterResources.September);
                    break;
                case 10:
                case 11:
                case 12:
                    duringWhichMonthRequestViewModel.Quarter.Add(10, @WhichQuarterResources.October);
                    duringWhichMonthRequestViewModel.Quarter.Add(11, @WhichQuarterResources.November);
                    duringWhichMonthRequestViewModel.Quarter.Add(12, @WhichQuarterResources.December);
                    break;
            }

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

        public WhatHaveYouDoneWasteModel GetWasteModel(int journeyId)
        {
            var whatHaveYouDoneWasteModel = new WhatHaveYouDoneWasteModel()
            {
                JourneyId = journeyId,
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

        public async Task SaveSelectedWasteType(int journeyId, string selectedWasteType)
        {
            await _httpWasteService.SaveSelectedWasteType(journeyId, selectedWasteType);
        }
    }
}