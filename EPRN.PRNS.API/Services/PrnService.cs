using AutoMapper;
using EPRN.Common.Dtos;
using EPRN.Common.Enums;
using EPRN.PRNS.API.Configuration;
using EPRN.PRNS.API.Repositories.Interfaces;
using EPRN.PRNS.API.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace EPRN.PRNS.API.Services
{
    public class PrnService : IPrnService
    {
        private readonly IMapper _mapper;
        private readonly IRepository _prnRepository;
        private readonly int? _currentMonthOverride; // overrides the current month. Required for testing purposes

        public PrnService(
            IOptions<AppConfigSettings> configSettings,
            IMapper mapper,
            IRepository prnRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _prnRepository = prnRepository ?? throw new ArgumentNullException(nameof(prnRepository));

            _currentMonthOverride = configSettings?.Value?.CurrentMonthOverride;
        }

        public async Task<int> CreatePrnRecord(
            int materialId,
            Category category)
        {
            return await _prnRepository.CreatePrnRecord(
                materialId, 
                category,
                GenerateReferenceNumber());
        }

        public async Task<double?> GetTonnage(int id)
        {
            return await _prnRepository.GetTonnage(id);
        }

        public async Task<bool> PrnRecordExists(
            int id,
            Category category)
        {
            return await _prnRepository.PrnExists(id, category);
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

        public async Task SaveCheckYourAnswers(int id, string reason)
        {
            await _prnRepository.UpdatePrnStatus(
                id, 
                PrnStatus.CheckYourAnswersComplete, reason);
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

        public async Task<PRNDetailsDto> GetPrnDetails(string reference)
        {
            return await _prnRepository.GetDetails(reference);
        }

        public async Task<DecemberWasteDto> GetDecemberWaste(int journeyId)
        {
            var monthToCheckAgainst = DateTime.Today.Month;

            if (_currentMonthOverride != null)
            {
                monthToCheckAgainst = _currentMonthOverride.Value;
            }

            if (monthToCheckAgainst == (int)Months.January || monthToCheckAgainst == (int)Months.December)
            {
                return await _prnRepository.GetDecemberWaste(journeyId);
            }

            return new DecemberWasteDto
            {
                Id = journeyId,
                IsWithinMonth = false,
            };
        }

        public async Task<PRNDetailsDto> GetPrnDetails(int id)
        {
            return await _prnRepository.GetDetails(id);
        }

        public async Task SaveDecemberWaste(int jouneyId, bool decemberWaste)
        {
            await _prnRepository.SaveDecemberWaste(jouneyId, decemberWaste);
        }

        public async Task SaveSentTo(int id, PrnStatus status)
        {
            await _prnRepository.SaveSentTo(id, status.ToString());
        }

    #region Private methods - Keep at bottom of file
    // Stub this and generate a random PRN reference number
    // In time a specific generation algorithm will
    // be specified
    private string GenerateReferenceNumber() => $"PRN{Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 10)}";
        #endregion
    }
}