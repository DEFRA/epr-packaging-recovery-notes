using AutoMapper;
using WasteManagement.API.Dtos;
using WasteManagement.API.Models;

namespace WasteManagement.API.Profiles
{
    public class WasteManagementProfile : Profile
    {
        public WasteManagementProfile()
        {
            CreateMap<DefaultTable, DefaultTableDto>();
        }
    }
}
