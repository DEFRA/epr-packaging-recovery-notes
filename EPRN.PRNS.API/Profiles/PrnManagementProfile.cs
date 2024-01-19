using AutoMapper;

namespace EPRN.PRNS.API.Profiles
{
    public class PrnManagementProfile : Profile
    {
        public PrnManagementProfile()
        {
            // Create Maps
            CreateMap<Common.Enums.Category, Common.Data.Enums.Category>();
            CreateMap<Common.Enums.PrnStatus, Common.Data.Enums.PrnStatus>();
        }
    }
}
