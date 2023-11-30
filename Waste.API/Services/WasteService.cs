﻿using AutoMapper;
using EPRN.Common.Dtos;
using EPRN.Common.Enum;
using Waste.API.Models;
using Waste.API.Repositories.Interfaces;
using Waste.API.Services.Interfaces;

namespace Waste.API.Services
{
    public class WasteService : IWasteService
    {
        public readonly IMapper _mapper;
        public readonly IRepository _wasteRepository;

        public WasteService(
            IMapper mapper,
            IRepository wasteRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _wasteRepository = wasteRepository ?? throw new ArgumentNullException(nameof(_wasteRepository));
        }

        public async Task<int> CreateJourney()
        {
            var journeyRecord = new WasteJourney
            {
                CreatedDate = DateTime.Now,
                CreatedBy = "DEVELOPER"
            };

            await _wasteRepository.Add(journeyRecord);

            return journeyRecord.Id;
        }

        public async Task SaveSelectedMonth(int journeyId, int selectedMonth)
        {
            var journeyRecord = await _wasteRepository.GetById<WasteJourney>(journeyId);

            if (journeyRecord == null)
                throw new ArgumentNullException(nameof(journeyRecord));

            journeyRecord.Month = selectedMonth;
            await _wasteRepository.Update(journeyRecord);
        }

        public async Task<IEnumerable<WasteTypeDto>> WasteTypes()
        {
            await Task.CompletedTask;
            // we want the entire table contents (at the
            // moment - there may be more requirements in the future)
            // so no where clause
            var dbWasteTypes = _wasteRepository
                .List<WasteType>()
                .OrderBy(wt => wt.Name)
                .ToList();

            return _mapper.Map<List<WasteTypeDto>>(dbWasteTypes);
        }

        public async Task SaveWasteType(int journeyId, int wasteTypeId)
        {
            var journeyRecord = await GetJourney(journeyId);
            if (journeyRecord == null)
                throw new ArgumentNullException(nameof(journeyRecord));

            journeyRecord.WasteTypeId = wasteTypeId;
            await _wasteRepository.Update(journeyRecord);
        }

        public async Task SaveWhatHaveYouDoneWaste(int journeyId, string whatHaveYouDoneWaste)
        {
            var journeyRecord = await GetJourney(journeyId);
            if (journeyRecord == null)
                throw new ArgumentNullException(nameof(journeyRecord));

            journeyRecord.DoneWaste = whatHaveYouDoneWaste;
            await _wasteRepository.Update(journeyRecord);
        }

        public async Task<string> GetWasteType(int journeyId)
        {
            await Task.CompletedTask;
            var wasteTypes = _wasteRepository
                .List<WasteType>(wt => wt.Journeys.Any(j => j.Id == journeyId));

            if (wasteTypes == null)
                throw new ArgumentNullException(nameof(wasteTypes));

            var wasteType = wasteTypes.FirstOrDefault();
            
            if (wasteType == null)
                throw new ArgumentNullException(nameof(wasteType));

            return wasteType.Name;
        }

        public async Task<WasteRecordStatusDto?> GetWasteRecordStatus(int journeyId)
        {
            var journey = await GetJourney(journeyId);

            if (journey == null)
                return null;

            var dto = new WasteRecordStatusDto
            {
                JourneyId = journey.Id,
                WasteBalance = GetWasteBalance(journey),
                WasteRecordReferenceNumber = string.IsNullOrWhiteSpace(journey.ReferenceNumber) ? string.Empty : journey.ReferenceNumber,
                WasteRecordStatus = EPRN.Common.Enums.WasteRecordStatuses.Incomplete
            };

            if (journey.Completed.HasValue)
                dto.WasteRecordStatus = journey.Completed.Value ? EPRN.Common.Enums.WasteRecordStatuses.Complete : EPRN.Common.Enums.WasteRecordStatuses.Incomplete;

            return dto;
        }

        private double GetWasteBalance(WasteJourney journey)
        {
            if (journey == null)
                return 0;

            return journey.Quantity.HasValue ? journey.Quantity.Value : 0;
        }

        private async Task<WasteJourney> GetJourney(int id)
        {
            return await _wasteRepository.GetById<WasteJourney>(id);
        }

        public async Task SaveBaledWithWire(int journeyId, YesNo baledWithWire)
        {
            var journeyRecord = await GetJourney(journeyId);
            if (journeyRecord == null)
                throw new ArgumentNullException(nameof(journeyRecord));

            journeyRecord.BaledWithWire = Convert.ToBoolean(baledWithWire);
            await _wasteRepository.Update(journeyRecord);
        }
    }
}
