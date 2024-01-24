using EPRN.Common.Dtos;

namespace EPRN.Portal.RESTServices.Interfaces
{
    public interface IHttpWasteService
    {
        Task<Dictionary<int, string>> GetWasteMaterialTypes();
        
        Task<IEnumerable<WasteSubTypeDto>> GetWasteMaterialSubTypes(int wasteTypeId);

    }
}
