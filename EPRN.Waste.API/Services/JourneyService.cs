using AutoMapper;
using EPRN.Common.Dtos;
using EPRN.Common.Enums;
using EPRN.Waste.API.Configuration;
using EPRN.Waste.API.Models;
using EPRN.Waste.API.Repositories.Interfaces;
using EPRN.Waste.API.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace EPRN.Waste.API.Services
{
    public class JourneyService : IJourneyService
    {
        private readonly double _deductionAmount;
        public readonly IMapper _mapper;
        public readonly IRepository _wasteRepository;

        public JourneyService(
            IMapper mapper,
            IRepository wasteRepository,
            IOptions<AppConfigSettings> configSettings)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _wasteRepository = wasteRepository ?? throw new ArgumentNullException(nameof(_wasteRepository));

            if (configSettings == null)
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

            await _wasteRepository.AddJourney(journeyRecord);

            return journeyRecord.Id;
        }

        public async Task<bool> JourneyExists(int journeyId)
        {
            return await _wasteRepository.Exists(journeyId);
        }

        public async Task SaveSelectedMonth(int journeyId, int selectedMonth)
        {
            await _wasteRepository.UpdateJourneyMonth(journeyId, selectedMonth);
        }

        public async Task SaveWasteType(int journeyId, int wasteTypeId)
        {
            await _wasteRepository.UpdateJourneyWasteTypeId(journeyId, wasteTypeId);
        }

        public async Task SaveWasteSubType(int journeyId, int wasteSubTypeId, double adjustment)
        {
            await _wasteRepository.UpdateJourneySubTypeAndAdjustment(journeyId, wasteSubTypeId, adjustment);
        }

        public async Task<DoneWaste?> GetWhatHaveYouDoneWaste(int journeyId)
        {
            return await _wasteRepository.GetDoneWaste(journeyId);
        }

        public async Task SaveWhatHaveYouDoneWaste(int journeyId, DoneWaste whatHaveYouDoneWaste)
        {
            await _wasteRepository.UpdateJourneyDoneId(journeyId, whatHaveYouDoneWaste);
        }

        public async Task<string> GetWasteType(int journeyId)
        {
            return await _wasteRepository.GetWasteTypeName(journeyId);
        }

        public async Task<WasteRecordStatusDto> GetWasteRecordStatus(int journeyId)
        {
            var journey = await _wasteRepository.GetWasteJourneyById(journeyId);

            if (journey == null)
                return null;

            var dto = new WasteRecordStatusDto
            {
                JourneyId = journey.Id,
                WasteBalance = journey.Quantity ?? 0,
                WasteRecordReferenceNumber = journey.ReferenceNumber,
                WasteRecordStatus = WasteRecordStatuses.Incomplete
            };

            if (journey.Completed.HasValue)
                dto.WasteRecordStatus = journey.Completed.Value ? WasteRecordStatuses.Complete : WasteRecordStatuses.Incomplete;

            return dto;
        }

        public async Task<double?> GetTonnage(int journeyId)
        {
            return await _wasteRepository.GetWasteTonnage(journeyId);
        }

        public async Task SaveTonnage(int journeyId, double tonnage)
        {
            await _wasteRepository.UpdateJourneyTonnage(journeyId, tonnage);
        }

        public async Task SaveBaledWithWire(int journeyId, bool baledWithWire)
        {
            await _wasteRepository.UpdateJourneyBaledWithWire(journeyId, baledWithWire);
        }

        public async Task SaveReprocessorExport(int journeyId, int siteId)
        {
            await _wasteRepository.UpdateJourneySiteId(journeyId, siteId);
        }

        public async Task<int?> GetWasteTypeId(int journeyId)
        {
            return await _wasteRepository.GetWasteTypeId(journeyId);
        }

        public async Task<WasteSubTypeSelectionDto> GetWasteSubTypeSelection(int journeyId)
        {
            return await _wasteRepository.GetWasteSubTypeSelection(journeyId);
        }
        
        public async Task<string> GetWasteRecordNote(int journeyId)
        {
            return await _wasteRepository.GetWasteNote(journeyId);
        }
    }
}