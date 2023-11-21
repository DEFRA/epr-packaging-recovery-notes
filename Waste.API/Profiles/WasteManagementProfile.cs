using AutoMapper;
using PRN.Common.Models;
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
