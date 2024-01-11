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
                    PRNReferenceNumber = string.IsNullOrWhiteSpace(prn.Reference) ? Guid.NewGuid().ToString().Replace("-", string.Empty) : prn.Reference,
                    PrnComplete = prn.CompletedDate.HasValue && prn.CompletedDate < DateTime.UtcNow,
                })
                .SingleOrDefaultAsync();
        }
    }
}