using AutoMapper;
using EPRN.Common.Dtos;
using Portal.ViewModels;

namespace Portal.Profiles
{
    public class WasteManagementProfile : Profile
    {
        public WasteManagementProfile()
        {
            CreateMap<WasteRecordStatusViewModel, WasteRecordStatusDto>();
        }
    }
}
