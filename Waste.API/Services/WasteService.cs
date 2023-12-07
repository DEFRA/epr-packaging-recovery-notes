using AutoMapper;
using EPRN.Common.Dtos;
using EPRN.Common.Enums;
using Microsoft.Extensions.Options;
using Waste.API.Configuration;
using Waste.API.Models;
using Waste.API.Repositories.Interfaces;
using Waste.API.Services.Interfaces;

namespace Waste.API.Services
{
    public class WasteService : IWasteService
    {
        private readonly double _deductionAmount;
        public readonly IMapper _mapper;
        public readonly IRepository _wasteRepository;

        public WasteService(
            IMapper mapper,
            IRepository wasteRepository,
            IOptions<AppConfigSettings> configSettings)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _wasteRepository = wasteRepository ?? throw new ArgumentNullException(nameof(_wasteRepository));
            
            if (configSettings == null )
                throw new ArgumentNullException(nameof(configSettings));

            if (configSettings.Value == null || configSettings.Value.DeductionAmount == null)
                throw new ArgumentNullException(nameof(configSettings.Value.DeductionAmount));

            _deductionAmount = configSettings.Value.DeductionAmount.Value; 
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

            if (journeyRecord.DoneWaste == DoneWaste.ReprocessedIt.ToString())
            {
                journeyRecord.MonthReceived = selectedMonth;
            }
            else
            {
                journeyRecord.MonthSent = selectedMonth;
            }

            await _wasteRepository.Update(journeyRecord);
        }

        public async Task<IEnumerable<WasteTypeDto>> WasteTypes()
        {
            _wasteRepository.LazyLoading = false;
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

        public async Task<DoneWaste> GetWhatHaveYouDoneWaste(int journeyId)
        {
            var journeyRecord = await GetJourney(journeyId);
            if (journeyRecord == null)
                throw new ArgumentNullException(nameof(journeyRecord));

            if (journeyRecord.DoneWaste == null)
                throw new ArgumentNullException(nameof(journeyRecord.DoneWaste));

            if (journeyRecord.DoneWaste == DoneWaste.ReprocessedIt.ToString())
                return DoneWaste.ReprocessedIt;
            else
                return DoneWaste.SentItOn;
        }

        public async Task SaveWhatHaveYouDoneWaste(int journeyId, DoneWaste whatHaveYouDoneWaste)
        {
            var journeyRecord = await GetJourney(journeyId);
            if (journeyRecord == null)
                throw new ArgumentNullException(nameof(journeyRecord));

            journeyRecord.DoneWaste = whatHaveYouDoneWaste.ToString();
            await _wasteRepository.Update(journeyRecord);
        }

        public async Task<string> GetWasteType(int journeyId)
        {
            var journeyRecord = await GetJourney(journeyId);
            if (journeyRecord == null)
                throw new ArgumentNullException(nameof(journeyRecord));

            if (journeyRecord.WasteTypeId == null)
                throw new ArgumentNullException(nameof(journeyRecord.WasteTypeId));

            return journeyRecord.WasteType.Name;
        }

        public async Task<WasteRecordStatusDto> GetWasteRecordStatus(int journeyId)
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

        public async Task SaveTonnage(int journeyId, double tonnage)
        {
            var journeyRecord = await GetJourney(journeyId);

            if (journeyRecord == null)
                throw new NullReferenceException(nameof(journeyRecord));

            journeyRecord.Tonnes = tonnage;
            await _wasteRepository.Update(journeyRecord);
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

        public async Task SaveBaledWithWire(int journeyId, bool baledWithWire)
        {
            var journeyRecord = await GetJourney(journeyId);
            if (journeyRecord == null)
                throw new NullReferenceException(nameof(journeyRecord));

            journeyRecord.BaledWithWire = baledWithWire;
            if (journeyRecord.BaledWithWire == true)
                journeyRecord.DeductionAmount = _deductionAmount;

            await _wasteRepository.Update(journeyRecord);
        }

        public async Task SaveReprocessorExport(int journeyId, int siteId)
        {
            var journeyRecord = await GetJourney(journeyId);
            if (journeyRecord == null)
                throw new NullReferenceException(nameof(journeyRecord));

            journeyRecord.SiteId = siteId;
            await _wasteRepository.Update(journeyRecord);

        }
    }
}
