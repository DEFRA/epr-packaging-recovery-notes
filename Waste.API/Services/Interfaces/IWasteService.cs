using EPRN.Common.Dtos;

namespace Waste.API.Services.Interfaces
{
    public interface IWasteService
    {
        Task<IEnumerable<WasteTypeDto>> WasteTypes();
    }
}
