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
        }
    }
}
