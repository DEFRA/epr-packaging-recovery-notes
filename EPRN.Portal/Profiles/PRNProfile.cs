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
        }
    }
}
