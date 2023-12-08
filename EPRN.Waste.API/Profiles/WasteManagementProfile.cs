using AutoMapper;
using EPRN.Common.Dtos;
using EPRN.Waste.API.Models;

namespace EPRN.Waste.API.Profiles
{
    public class WasteManagementProfile : Profile
    {
        public WasteManagementProfile()
        {
            CreateMap<WasteType, WasteTypeDto>();
            CreateMap<WasteSubType, WasteSubTypeDto>();
        }
    }
}
