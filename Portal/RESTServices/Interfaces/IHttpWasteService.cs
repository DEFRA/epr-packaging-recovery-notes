using EPRN.Common.Dtos;

namespace Portal.RESTServices.Interfaces
{
    public interface IHttpWasteService
    {
        Task<IEnumerable<WasteTypeDto>> GetWasteMaterialTypes();

        Task SaveSelectedMonth(int journeyId, int selectedMonth);
    }
}
