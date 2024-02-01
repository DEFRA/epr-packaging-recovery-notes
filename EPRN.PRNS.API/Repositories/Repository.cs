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
                    MaterialName = prn.WasteTypeId.HasValue ? prn.WasteType.Name : string.Empty,
                    Tonnage = prn.Tonnes,
                    Notes = prn.Note,
                    RecipientName = prn.SentTo
                })
                .SingleOrDefaultAsync();
        }

        public async Task<Common.Enums.PrnStatus> GetStatus(int id)
        {
            return await _prnContext
                .PRN
                .Where(prn => prn.Id == id)
                .Select(prn => _mapper.Map<Common.Enums.PrnStatus>(prn.Status))
                .SingleOrDefaultAsync();
        }

        public async Task<StatusAndProducerDto> GetStatusAndRecipient(int id)
        {
            return await _prnContext
                .PRN
                .Where(prn => prn.Id == id)
                .Select(prn => new StatusAndProducerDto
                {
                    Id = prn.Id,
                    Status = _mapper.Map<Common.Enums.PrnStatus>(prn.Status),
                    Producer = prn.SentTo
                })
                .SingleOrDefaultAsync();
        }

        public async Task UpdatePrnStatus(
            int id, 
            Common.Enums.PrnStatus status,
            string reason = null)
        {
            await _prnContext
                .PRN
                .Where(prn => prn.Id == id)
                .ExecuteUpdateAsync(sp => sp
                    .SetProperty(prn => prn.Status, _mapper.Map<PrnStatus>(status))
                    .SetProperty(prn => prn.StatusReason, reason)
                );
        }

        public async Task<SentPrnsDto> GetSentPrns(GetSentPrnsDto request)
        {
            var recordsPerPage = request.PageSize;

            var prns = _prnContext.PRN
                .Include(repo => repo.WasteType)
                .AsQueryable();

            prns = prns.OrderByDescending(repo => repo.CreatedDate);

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                prns = prns.Where(repo =>
                    EF.Functions.Like(repo.Reference, $"%{request.SearchTerm}%") ||
                    EF.Functions.Like(repo.SentTo, $"%{request.SearchTerm}%"));
            }

            if (!string.IsNullOrWhiteSpace(request.FilterBy))
                prns = prns.Where(e => (int)e.Status == int.Parse(request.FilterBy));

            if (request.SortBy == "1")
                prns = prns.OrderByDescending(e => e.WasteType);
            else if (request.SortBy == "2")
                prns = prns.OrderBy(e => e.SentTo);

            // get the count BEFORE paging
            var totalRecords = await prns.CountAsync();

            prns = prns
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize);

            var totalPages = (totalRecords + recordsPerPage - 1) / recordsPerPage;

            return new SentPrnsDto()
            {
                Rows = await prns.Select(prn => new PrnDto
                {
                    PrnNumber = prn.Reference,
                    Material = prn.WasteTypeId.HasValue ? prn.WasteType.Name : string.Empty,
                    SentTo = prn.SentTo,
                    DateCreated = prn.CreatedDate.ToShortDateString(),
                    Tonnes = prn.Tonnes.Value,
                    Status = _mapper.Map<Common.Enums.PrnStatus>(prn.Status)
                }).ToListAsync(),

                Pagination = new PaginationDto
                {
                    TotalItems = totalRecords,
                    CurrentPage = request.Page,
                    ItemsPerPage = recordsPerPage,
                    TotalPages = totalPages,
                },

                SearchTerm = request.SearchTerm,
                FilterBy = request.FilterBy,
                SortBy = request.SortBy
            };
        }
    }
}