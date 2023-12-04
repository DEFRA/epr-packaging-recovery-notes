using AutoMapper;
using EPRN.Common.Dtos;
using EPRN.Portal.ViewModels;

namespace EPRN.Portal.Profiles
{
    public class WasteManagementProfile : Profile
    {
        public WasteManagementProfile()
        {
            CreateMap<WasteRecordStatusViewModel, WasteRecordStatusDto>();
        }
    }
}
