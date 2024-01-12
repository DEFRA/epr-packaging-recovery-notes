using AutoMapper;
using EPRN.Common.Data.DataModels;
using EPRN.Common.Dtos;

namespace EPRN.PRNS.API.Profiles
{
    public class PrnManagementProfile : Profile
    {
        public PrnManagementProfile()
        {
            // Create Maps
            CreateMap<PackagingRecoveryNote, ConfirmationDto>()            
                .ForMember(d => d.PRNReferenceNumber, o => o.MapFrom(s =>
                    // we have no prn reference number functionality, so for now just make something up
                    string.IsNullOrWhiteSpace(s.Reference) ? $"PRN{Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 10)}" : s.Reference))
                .ForMember(d => d.PrnComplete, o => o.MapFrom(s => s.CompletedDate.HasValue && s.CompletedDate.Value < DateTime.Now))
                .ForMember(d => d.CompanyNameSentTo, o => o.MapFrom(s =>
                    // if it's null, the PRN is not complete, so we won't be using the id field anyway
                    s.SentTo ?? string.Empty)); 
        }
    }
}
