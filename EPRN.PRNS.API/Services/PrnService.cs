using AutoMapper;
using EPRN.Common.Dtos;
using EPRN.Common.Enums;
using EPRN.PRNS.API.Repositories.Interfaces;
using EPRN.PRNS.API.Services.Interfaces;

namespace EPRN.PRNS.API.Services
{
    public class PrnService : IPrnService
    {
        public readonly IMapper _mapper;
        public readonly IRepository _prnRepository;

        public PrnService(
            IMapper mapper,
            IRepository prnRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _prnRepository = prnRepository ?? throw new ArgumentNullException(nameof(prnRepository));
        }

        public async Task<int> CreatePrnRecord(
            int materialId,
            Category category)
        {
            return await _prnRepository.CreatePrnRecord(materialId, category);
        }

        public async Task<double?> GetTonnage(int id)
        {
            return await _prnRepository.GetTonnage(id);
        }

        public async Task<bool> PrnRecordExists(
            int id)
        {
            return await _prnRepository.PrnExists(id);
        }

        public async Task SaveTonnage(int id, double tonnage)
        {
            await _prnRepository.UpdateTonnage(id, tonnage);
        }

        public async Task<ConfirmationDto> GetConfirmation(int id)
        {
            return await _prnRepository.GetConfirmation(id);
        }

        public async Task<CheckYourAnswersDto> GetCheckYourAnswers(int id)
        {
            return await _prnRepository.GetCheckYourAnswersData(id);
        }

        public async Task<PrnStatus> GetStatus(int id)
        {
            return await _prnRepository.GetStatus(id);
        }

        public async Task<StatusAndProducerDto> GetStatusWithProducerName(int id)
        {
            return await _prnRepository.GetStatusAndRecipient(id);
        }

        public async Task SaveCheckYourAnswers(int id)
        {
            await _prnRepository.UpdatePrnStatus(
                id, 
                PrnStatus.CheckYourAnswersComplete);
        }

        public async Task CancelPrn(int id, string reason)
        {
            // this needs to be for a not accepted PRN... 
            // not sure where to do that yet
            if (await _prnRepository.GetStatus(id) != PrnStatus.Accepted)
            {
                await _prnRepository.UpdatePrnStatus(
                    id,
                    PrnStatus.Cancelled,
                    reason);
            }
        }

        public async Task RequestCancelPrn(int id, string reason)
        {
            // this needs to be for an accepted PRN... 
            // not sure where to do that yet
            if (await _prnRepository.GetStatus(id) == PrnStatus.Accepted)
            {
                await _prnRepository.UpdatePrnStatus(
                    id,
                    PrnStatus.CancellationRequested,
                    reason);
            }
        }

        public async Task<SentPrnsDto> GetSentPrns(GetSentPrnsDto request)
        {
            return await _prnRepository.GetSentPrns(request);
        }

        public async Task<DecemberWasteDto> GetDecemberWaste(int journeyId)
        {
            var decemberWaste = await _prnRepository.GetDecemberWaste(journeyId);

            return _mapper.Map<DecemberWasteDto>(decemberWaste);
        }

        public async Task SaveDecemberWaste(int jouneyId, bool decemberWaste)
        {
            await _prnRepository.SaveDecemberWaste(jouneyId, decemberWaste);
        }
    }
}