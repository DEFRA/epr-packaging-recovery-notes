using EPRN.Common.Data;
using EPRN.Common.Data.DataModels;
using EPRN.Common.Dtos;
using EPRN.PRNS.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPRN.PRNS.API.Repositories
{
    public class Repository : IRepository
    {
        private readonly EPRNContext _prnContext;

        public Repository(EPRNContext prnContext)
        {
            _prnContext = prnContext ?? throw new ArgumentNullException(nameof(prnContext));
        }

        public async Task<int> CreatePrnRecord()
        {
            throw new NotImplementedException();
        }

        public Task<PackagingRecoveryNote> GetPrnById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> PrnExists(int id)
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
                    Id = prn.Id,
                    // Reference number will be generated in a seperate story, and
                    // at the point that the completed date is set
                    PRNReferenceNumber = string.IsNullOrWhiteSpace(prn.Reference) ? $"PRN{Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 10)}" : prn.Reference,
                    PrnComplete = prn.CompletedDate.HasValue && prn.CompletedDate < DateTime.UtcNow,
                })
                .SingleOrDefaultAsync();
        }
    }
}