using AutoMapper;
using EPRN.Common.Dtos;
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

        public async Task SaveSelectedWasteType(int journeyId, string selectedWasteType)
        {
            var journeyRecord = await GetJourney(journeyId);
            if (journeyRecord == null)
                throw new ArgumentNullException(nameof(journeyRecord));

            journeyRecord.Note = selectedWasteType;
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

        private async Task<WasteJourney> GetJourney(int id)
        {
            return await _wasteRepository.GetById<WasteJourney>(id);
        }
    }
}
