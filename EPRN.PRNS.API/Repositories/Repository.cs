using EPRN.Common.Data.DataModels;
using EPRN.PRNS.API.Data;
using EPRN.PRNS.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EPRN.PRNS.API.Repositories
{
    public class Repository : IRepository
    {
        private readonly PrnContext _prnContext;

        public Repository(PrnContext prnContext)
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
                .Where(wj => wj.Id == id)
                .Select(wj => wj.Tonnes)
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
    }
}