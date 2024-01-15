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
                Category = _mapper.Map<Category>(category)
            };
            
            _prnContext.Add(prn);
            await _prnContext.SaveChangesAsync();

            return prn.Id;
        }

        public async Task<bool> PrnExists(
            int id,
            Common.Enums.Category category)
        {
            return await _prnContext
                .PRN
                .AnyAsync(p => p.Id == id && p.Category == _mapper.Map<Category>(category));
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
                .Select(prn => _mapper.Map<ConfirmationDto>(prn))
                .SingleOrDefaultAsync();
        }
    }
}