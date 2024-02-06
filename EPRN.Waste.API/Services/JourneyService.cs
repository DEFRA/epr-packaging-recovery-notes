using AutoMapper;
using EPRN.Common.Data.DataModels;
using EPRN.Common.Data.Enums;
using EPRN.Common.Dtos;
using EPRN.Common.Enums;
using EPRN.Waste.API.Configuration;
using EPRN.Waste.API.Repositories.Interfaces;
using EPRN.Waste.API.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Globalization;
using DoneWaste = EPRN.Common.Enums.DoneWaste;
using Category = EPRN.Common.Enums.Category;

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

        public async Task<int> CreateJourney(
            int materialId,
            Category category)
        {
            var journeyRecord = new WasteJourney
            {
                WasteTypeId = materialId,
                Category = _mapper.Map<Common.Data.Enums.Category>(category),
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
            return await _wasteRepository.GetWasteRecordStatus(journeyId);
        }

        public async Task<double?> GetTonnage(int journeyId)
        {
            return await _wasteRepository.GetWasteTonnage(journeyId);
        }

        public async Task SaveTonnage(int journeyId, double tonnage)
        {
            await _wasteRepository.UpdateJourneyTonnage(journeyId, tonnage);
        }

        public async Task<BaledWithWireDto> GetBaledWithWire(int journeyId)
        {
            var dto = new BaledWithWireDto { JourneyId = journeyId };

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
            return await _wasteRepository.GetWasteSubTypeSelection(journeyId);
        }

        public async Task<NoteDto> GetWasteRecordNote(int journeyId)
        {
            return await _wasteRepository.GetWasteNote(journeyId);
        }

        public async Task<JourneyAnswersDto> GetJourneyAnswers(int journeyId)
        {
            return await _wasteRepository.GetWasteJourneyAnswersById(journeyId);
        }

        public async Task SaveWasteRecordNote(int journeyId, string note)
        {
            await _wasteRepository.UpdateWasteNote(journeyId, note);
        }

        public async Task<object?> GetCategory(int journeyId)
        {
            var category = await _wasteRepository.GetCategory(journeyId);

            return _mapper.Map<Category>(category);
        }
    }
}