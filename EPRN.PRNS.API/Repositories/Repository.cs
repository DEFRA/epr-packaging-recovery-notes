using AutoMapper;
using EPRN.Common.Data;
using EPRN.Common.Data.DataModels;
using EPRN.Common.Data.Enums;
using EPRN.Common.Dtos;
using EPRN.PRNS.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPRN.PRNS.API.Repositories
{
    public class Repository : IRepository
    {
        private readonly IMapper _mapper;
        private readonly EPRNContext _prnContext;

        public Repository(
            IMapper mapper,
            EPRNContext prnContext)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _prnContext = prnContext ?? throw new ArgumentNullException(nameof(prnContext));
        }

        public async Task<int> CreatePrnRecord(
            int materialType,
            Common.Enums.Category category)
        {
            var prn = new PackagingRecoveryNote
            {
                WasteTypeId = materialType,
                Category = _mapper.Map<Category>(category),
                Status = PrnStatus.Draft
            };
            
            _prnContext.Add(prn);
            await _prnContext.SaveChangesAsync();

            return prn.Id;
        }

        public async Task<bool> PrnExists(
            int id)
        {
            return await _prnContext
                .PRN
                .AnyAsync(p => p.Id == id);
        }

        public async Task<double?> GetTonnage(int id)
        {
            return await _prnContext
                .PRN
                .Where(prn => prn.Id == id)
                .Select(prn => prn.Tonnes)
                .SingleOrDefaultAsync();
        }

        public async Task UpdateTonnage(int id, double tonnes)
        {
            await _prnContext
                .PRN
                .Where(prn => prn.Id == id)
                .ExecuteUpdateAsync(sp =>
                    sp.SetProperty(prn => prn.Tonnes, tonnes)
                );
        }

        public async Task<ConfirmationDto> GetConfirmation(int id)
        {
            return await _prnContext
                .PRN
                .Where(prn => prn.Id == id)
                .Select(prn => new ConfirmationDto
                {
                    PRNReferenceNumber = string.IsNullOrWhiteSpace(prn.Reference) ? 
                        $"PRN{Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 10)}" : prn.Reference,
                    PrnComplete = prn.CompletedDate.HasValue && prn.CompletedDate.Value < DateTime.Now,
                    CompanyNameSentTo = prn.SentTo ?? string.Empty
                })
                .SingleOrDefaultAsync();
        }

        public async Task<CheckYourAnswersDto> GetCheckYourAnswersData(int id)
        {
            return await _prnContext
                .PRN
                .Where(prn => prn.Id == id)
                .Select(prn => new CheckYourAnswersDto
                {
                    Id = prn.Id,
                    MaterialName = prn.WasteTypeId.HasValue ? prn.WasteType.Name: string.Empty,
                    Tonnage = prn.Tonnes,
                    Notes = prn.Note,
                    RecipientName = prn.SentTo
                })
                .SingleOrDefaultAsync();
        }

        public async Task UpdatePrnStatus(
            int id, 
            Common.Enums.PrnStatus status)
        {
            await _prnContext
                .PRN
                .Where(prn => prn.Id == id)
                .ExecuteUpdateAsync(sp =>
                    sp.SetProperty(prn => prn.Status, _mapper.Map<PrnStatus>(status))
                );
        }

        public async Task<DecemberWasteDto> GetDecemberWaste(int journeyId)
        {
            var decemberWaste = _prnContext.PRN
                .Where(wj => wj.Id == journeyId)
                .Select(x => new DecemberWasteDto
                {
                    JourneyId = journeyId,
                    DecemberWaste = x.DecemberWaste
                })
                .SingleOrDefaultAsync();

            return new DecemberWasteDto()
            {
                DecemberWaste = decemberWaste.Result.DecemberWaste,
                JourneyId = journeyId
            };
        }

        public async Task SaveDecemberWaste(int journeyId, bool decemberWaste)
        {
            await _prnContext.PRN
                .Where(wj => wj.Id == journeyId)
                .ExecuteUpdateAsync(sp =>
                    sp.SetProperty(wj => wj.DecemberWaste, decemberWaste)
                );
        }
    }
}