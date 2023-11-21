using PRN.Common.Models;

namespace Waste.API.Services.Interfaces
{
    public interface IWasteService
    {
        IEnumerable<WasteTypeDto> GetWasteTypes();
    }
}
