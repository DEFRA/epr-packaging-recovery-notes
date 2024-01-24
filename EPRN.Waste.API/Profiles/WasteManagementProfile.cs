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
            CreateMap<Common.Data.Enums.Category, Common.Enums.Category>()
                .ReverseMap();
            CreateMap<Common.Data.Enums.DoneWaste, Common.Enums.DoneWaste>()
                .ReverseMap();
            CreateMap<WasteJourney, WasteRecordStatusDto>()
                .ForMember(d => d.JourneyId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.WasteBalance, o => o.MapFrom(s => s.Quantity ?? 0))
                .ForMember(d => d.WasteRecordReferenceNumber, o => o.MapFrom(o => o.ReferenceNumber ?? "WR00012"))
                .ForMember(d => d.WasteRecordStatus, o => o.MapFrom(o =>
                    o.Completed.HasValue ? (o.Completed.Value ? WasteRecordStatuses.Complete : WasteRecordStatuses.Incomplete) : WasteRecordStatuses.Incomplete));
            CreateMap<WasteJourney, GetBaledWithWireDto>()
                .ForMember(d => d.JourneyId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.BaledWithWire, o => o.MapFrom(s => s.BaledWithWire))
                .ForMember(d => d.BaledWithWireDeductionPercentage, o => o.MapFrom(s => s.BaledWithWireDeductionPercentage));
        }
    }
}
