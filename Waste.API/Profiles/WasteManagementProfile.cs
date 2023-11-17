using AutoMapper;
using Waste.API.Dtos;
using Waste.API.Models;

namespace WasteManagement.API.Profiles
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
