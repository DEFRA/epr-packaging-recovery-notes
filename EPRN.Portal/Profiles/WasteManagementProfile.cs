using AutoMapper;
using EPRN.Common.Dtos;
using EPRN.Portal.ViewModels.Waste;

namespace EPRN.Portal.Profiles
{
    public class WasteManagementProfile : Profile
    {
        public WasteManagementProfile()
        {
            CreateMap<WasteRecordStatusViewModel, WasteRecordStatusDto>().ReverseMap();
            CreateMap<WasteSubTypeOptionViewModel, WasteSubTypeDto>().ReverseMap();
            CreateMap<BaledWithWireViewModel, BaledWithWireDto>().ReverseMap();

        }
    }
}
