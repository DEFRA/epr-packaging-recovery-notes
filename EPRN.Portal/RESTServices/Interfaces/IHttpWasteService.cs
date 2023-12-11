using EPRN.Common.Dtos;
using EPRN.Common.Enums;

namespace EPRN.Portal.RESTServices.Interfaces
{
    public interface IHttpWasteService
    {
        Task<IEnumerable<WasteTypeDto>> GetWasteMaterialTypes();
        
        Task<IEnumerable<WasteSubTypeDto>> GetWasteMaterialSubTypes(int wasteTypeId);
    }
}
