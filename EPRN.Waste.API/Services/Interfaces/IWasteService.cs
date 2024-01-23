using EPRN.Common.Dtos;

namespace EPRN.Waste.API.Services.Interfaces
{
    public interface IWasteService
    {
        Task<Dictionary<int, string>> WasteTypes();

        Task<IEnumerable<WasteSubTypeDto>> WasteSubTypes(int wasteTypeid);
    }
}
