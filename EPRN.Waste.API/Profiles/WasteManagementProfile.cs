using AutoMapper;
using EPRN.Common.Data.DataModels;
using EPRN.Common.Dtos;
using EPRN.Common.Enums;

namespace EPRN.Waste.API.Profiles
{
    public class WasteManagementProfile : Profile
    {
        public WasteManagementProfile()
        {
            CreateMap<WasteType, WasteTypeDto>();
            CreateMap<WasteSubType, WasteSubTypeDto>();
            CreateMap<Common.Data.DoneWaste, DoneWaste>();
            CreateMap<DoneWaste, Common.Data.DoneWaste>();
        }
    }
}
