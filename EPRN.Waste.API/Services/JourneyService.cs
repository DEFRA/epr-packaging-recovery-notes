using AutoMapper;
using EPRN.Common.Data.DataModels;
using EPRN.Common.Dtos;
using EPRN.Common.Enums;
using EPRN.Waste.API.Configuration;
using EPRN.Waste.API.Repositories.Interfaces;
using EPRN.Waste.API.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Globalization;

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

        public async Task<int?> GetSelectedMonth(int journeyId)
        {
            return await _wasteRepository.GetSelectedMonth(journeyId);
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
            var doneWaste = await _wasteRepository.GetDoneWaste(journeyId);

            return _mapper.Map<DoneWaste>(doneWaste);
        }

        public async Task SaveWhatHaveYouDoneWaste(int journeyId, DoneWaste whatHaveYouDoneWaste)
        {
            var doneWaste = _mapper.Map<Common.Data.Enums.DoneWaste>(whatHaveYouDoneWaste);
            await _wasteRepository.UpdateJourneyDoneId(journeyId, doneWaste);
        }

        public async Task<string> GetWasteType(int journeyId)
        {
            return await _wasteRepository.GetWasteTypeName(journeyId);
        }

        public async Task<WasteRecordStatusDto> GetWasteRecordStatus(int journeyId)
        {
            var journey = await _wasteRepository.GetWasteJourneyById_FullModel(journeyId);

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

        public async Task<GetBaledWithWireDto> GetBaledWithWire(int journeyId)
        {
            var dto = new GetBaledWithWireDto { JourneyId = journeyId };

            var journey = await _wasteRepository.GetWasteJourneyById_FullModel(journeyId);
            if (journey != null)
            {
                dto.BaledWithWire = journey.BaledWithWire;
                dto.BaledWithWireDeductionPercentage = journey.BaledWithWireDeductionPercentage;
            }

            return dto;
        }

        public async Task SaveBaledWithWire(int journeyId, bool baledWithWire, double baledWithWireDeductionPercentage)
        {
            await _wasteRepository.UpdateJourneyBaledWithWire(journeyId, baledWithWire, baledWithWireDeductionPercentage);
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
            var journey = await _wasteRepository.GetWasteSubTypeSelection(journeyId);
            if (journey == null)
            {
                throw new Exception(nameof(journey));
            }

            if (!journey.WasteSubTypeId.HasValue)
            {
                throw new Exception(nameof(journey));
            }

            var wasteSubTypeSelection = new WasteSubTypeSelectionDto
            {
                WasteSubTypeId = journey.WasteSubTypeId.Value,
                Adjustment = journey.Adjustment
            };

            return wasteSubTypeSelection;
        }

        public async Task<string> GetWasteRecordNote(int journeyId)
        {
            return await _wasteRepository.GetWasteNote(journeyId);
        }

        public async Task<JourneyAnswersDto> GetJourneyAnswers(int journeyId)
        {
            var journey = await _wasteRepository.GetWasteJourneyById_FullModel(journeyId);
            if (journey == null)
                throw new Exception(nameof(journey));

            var journeyAnswersDto = new JourneyAnswersDto();

            journeyAnswersDto.WhatDoneWithWaste = journey.DoneWaste == null ? string.Empty : journey.DoneWaste.Value.ToString();
            journeyAnswersDto.BaledWithWire = journey.BaledWithWire == null ? string.Empty : (journey.BaledWithWire.Value == true ? "Yes" : "No");
            journeyAnswersDto.Month = journey.Month == null ? string.Empty : CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(journey.Month.Value);
            journeyAnswersDto.WasteType = journey.WasteType == null ? string.Empty : journey.WasteType.Name.ToString();
            journeyAnswersDto.Tonnes = journey.Tonnes == null ? string.Empty : journey.Tonnes.Value.ToString();
            journeyAnswersDto.TonnageAdjusted = journey.Adjustment == null ? string.Empty : journey.Adjustment.Value.ToString();
            journeyAnswersDto.Note = journey.Note;
            journeyAnswersDto.WasteSubType = journey.WasteSubType == null ? string.Empty : journey.WasteSubType.Name.ToString();
            journeyAnswersDto.Completed = journey.Completed == null ? false : journey.Completed.Value;

            return journeyAnswersDto;
        }
    }
}