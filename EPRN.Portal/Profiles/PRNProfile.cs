using AutoMapper;
using EPRN.Common.Dtos;
using EPRN.Portal.ViewModels.PRNS;

namespace EPRN.Portal.Profiles
{
    public class PRNProfile : Profile
    {
        public PRNProfile()
        {
            CreateMap<ConfirmationDto, ConfirmationViewModel>();
            CreateMap<CheckYourAnswersDto, CheckYourAnswersViewModel>();
            CreateMap<StatusAndProducerDto, RequestCancelViewModel>()
                .ForMember(d => d.Regulator, o => o.Ignore()); // we don't know where the regulator is coming from yet
            CreateMap<PrnDto, PrnRowViewModel>();
            CreateMap<PaginationDto, PaginationViewModel>();
            CreateMap<SentPrnsDto, ViewSentPrnsViewModel>();
            CreateMap<GetSentPrnsViewModel, GetSentPrnsDto>();
            CreateMap<DecemberWasteDto, DecemberWasteViewModel>()
                .ForMember(d => d.WasteForDecember, o => o.MapFrom(s => s.DecemberWaste));

            CreateMap<PRNDetailsDto, ViewPRNViewModel>()
                .ForMember(d => d.SiteAddress, o => o.MapFrom(s => s.SiteAddress.Replace(Environment.NewLine, "<br/>")))
                .ForMember(d => d.History, o => o.MapFrom(s => s.History.OrderBy(h => h.Created)))
                // get the current status from the history records. It's the most history record
                .ForMember(d => d.Status, o => o.MapFrom(s =>
                    s.History
                    .OrderByDescending(h => h.Created)
                    .Select(h => h.Status)
                    .First()));

            CreateMap<PRNHistoryDto, PRNHistoryViewModel>();
            CreateMap<StatusAndProducerDto, CancelViewModel>();
            CreateMap<PRNDetailsDto, DraftConfirmationViewModel>()
                .ForMember(d => d.DoWithPRN, o => o.MapFrom(s =>
                    s.History
                    .Select(h => h.Status)
                    .First()));
        }
    }
}