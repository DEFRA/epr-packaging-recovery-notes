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
                .SingleOrDefaultAsync();
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
                    Producer = h.PackagingRecoveryNote.SentTo
                })
                .SingleOrDefaultAsync();
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
    }
}