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

        // we will need to identify the username when account system is implemented
        // most likely we could inject this from somesort of user management class
        private const string username = "PRN USER";

        public Repository(
            IMapper mapper,
            EPRNContext prnContext)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _prnContext = prnContext ?? throw new ArgumentNullException(nameof(prnContext));
        }

        public async Task<int> CreatePrnRecord(
            int materialType,
            Common.Enums.Category category,
            string prnReference)
        {
            var prn = new PackagingRecoveryNote
            {
                WasteTypeId = materialType,
                Category = _mapper.Map<Category>(category),
                Reference = prnReference,
                PrnHistory = new List<PrnHistory>
                {
                    new PrnHistory
                    {
                        Status = PrnStatus.Draft,
                        Created = DateTime.UtcNow,
                        CreatedBy = username,
                        Reason = "Created"
                    }
                }
            };

            await _prnContext.AddAsync(prn);
            await _prnContext.SaveChangesAsync();

            return prn.Id;
        }

        public async Task<bool> PrnExists(
            int id,
            EPRN.Common.Enums.Category category)
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
                .Select(prn => new ConfirmationDto
                {
                    PRNReferenceNumber = prn.Reference,
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
            // This is the status on the most recent PrnHistory record
            return await _prnContext.PRNHistory
                .Where(h => h.PrnId == id)
                .OrderByDescending(h => h.Created)
                .Select(h => _mapper.Map<Common.Enums.PrnStatus>(h.Status))
                .FirstOrDefaultAsync();
        }

        public async Task<StatusAndProducerDto> GetStatusAndRecipient(int id)
        {
            return await _prnContext
                .PRNHistory
                .Where(h => h.PrnId == id)
                .OrderByDescending(h => h.Created)
                .Select(h => new StatusAndProducerDto
                {
                    Id = h.PackagingRecoveryNote.Id,
                    Status = _mapper.Map<Common.Enums.PrnStatus>(h.Status),
                    Producer = h.PackagingRecoveryNote.SentTo,
                    Reference = h.PackagingRecoveryNote.Reference
                })
                .FirstOrDefaultAsync();
        }

        public async Task UpdatePrnStatus(
            int id,
            Common.Enums.PrnStatus status,
            string reason = null)
        {
            var historyRecord = new PrnHistory
            {
                PrnId = id,
                CreatedBy = username,
                Created = DateTime.UtcNow,
                Status = _mapper.Map<PrnStatus>(status),
                Reason = reason
            };

            await _prnContext.AddAsync(historyRecord);
            await _prnContext.SaveChangesAsync();
        }

        public async Task<SentPrnsDto> GetSentPrns(GetSentPrnsDto request)
        {
            var recordsPerPage = request.PageSize;
            var prns = _prnContext.PRN
                .Include(repo => repo.WasteType)
                .Include(repo => repo.PrnHistory)
                .Where(prn => prn.PrnHistory.Any(h => h.Status >= PrnStatus.Accepted));
            if (request.FilterBy.HasValue)
            {
                // map the filterby value to the Data version of the enum
                var filterByStatus = _mapper.Map<PrnStatus>(request.FilterBy);
                prns = prns.Where(e =>
                    e.PrnHistory != null &&
                    e.PrnHistory.Any() &&
                    e.PrnHistory.OrderByDescending(h => h.Created).First().Status == filterByStatus);
            }
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                prns = prns.Where(repo =>
                    EF.Functions.Like(repo.Reference, $"%{request.SearchTerm}%") ||
                    EF.Functions.Like(repo.SentTo, $"%{request.SearchTerm}%"));
            }

            // get the count BEFORE paging and sorting
            var totalRecords = await prns.CountAsync();

            // Apply sorting after all filters
            if (request.SortBy == "1")
                prns = prns.OrderBy(e => e.WasteType.Name);
            else if (request.SortBy == "2")
                prns = prns.OrderBy(e => e.SentTo);
            else
                prns = prns.OrderByDescending(repo => repo.CreatedDate);

            // Apply pagination after sorting
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
                    Tonnes = prn.Tonnes,
                    Status = prn.PrnHistory != null && prn.PrnHistory.Any()
                        ? (Common.Enums.PrnStatus)prn.PrnHistory.OrderByDescending(h => h.Created).First().Status
                        : default
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


        public async Task<PRNDetailsDto> GetDetails(string reference)
        {
            return await _prnContext
                .PRN
                .Where(prn => prn.Reference == reference)
                .Select(prn => new PRNDetailsDto
                {
                    AccreditationNumber = "UNKNOWN",
                    ReferenceNumber = prn.Reference,
                    SiteAddress = $"Unknown street{Environment.NewLine}Unknown town{Environment.NewLine}Unknown postcode",
                    CreatedBy = prn.PrnHistory.First(h => h.Status == PrnStatus.Draft).CreatedBy,
                    DecemberWasteBalance = false,
                    Tonnage = prn.Tonnes,
                    DateSent = prn.PrnHistory.Where(h => h.Status == PrnStatus.Accepted).Select(h => h.Created).FirstOrDefault(),
                    SentTo = prn.SentTo,
                    Note = prn.Note,
                    History =
                        // get the history 
                        prn
                        .PrnHistory
                        .OrderByDescending(h => h.Created)
                        .Select(h => new PRNHistoryDto
                        {
                            Created = h.Created,
                            Reason = h.Reason,
                            Status = _mapper.Map<Common.Enums.PrnStatus>(h.Status),
                            Username = h.CreatedBy
                        })
                })
                .SingleOrDefaultAsync();
        }
        public async Task<DecemberWasteDto> GetDecemberWaste(int id)
        {
            return await _prnContext.PRN
                .Where(prn => prn.Id == id)
                .Select(prn => new DecemberWasteDto
                {
                    Id = id,
                    DecemberWaste = prn.DecemberWaste,
                    IsWithinMonth = true
                })
                .SingleOrDefaultAsync();
        }

        public async Task SaveDecemberWaste(int journeyId, bool decemberWaste)
        {
            await _prnContext.PRN
                .Where(wj => wj.Id == journeyId)
                .ExecuteUpdateAsync(sp =>
                    sp.SetProperty(wj => wj.DecemberWaste, decemberWaste)
                );
        }

        public async Task<DeleteDraftPrnDto> GetPrnReference(int id)
        {
            return await _prnContext
                .PRN
                .Where(prn => prn.Id == id)
                .Select(prn => new DeleteDraftPrnDto
                {
                    Id = prn.Id,
                    PrnReference = prn.Reference
                })
                .FirstOrDefaultAsync();
        }

        public async Task DeleteDraftPrn(int id)
        {
            await _prnContext
                .PRN
                .Where(prn => prn.Id == id)
                .ExecuteUpdateAsync(sp =>
                    sp.SetProperty(prn => prn.IsDeleted, true)
                );
        }
    }
}