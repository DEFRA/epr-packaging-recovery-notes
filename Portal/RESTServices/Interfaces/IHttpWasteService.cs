using EPRN.Common.Dtos;
using Portal.ViewModels;

namespace Portal.RESTServices.Interfaces
{
    public interface IHttpWasteService
    {
        Task<IEnumerable<WasteTypeDto>> GetWasteMaterialTypes();
        Task<WasteRecordStatusViewModel?> GetWasteRecordStatus(string reprocessorId);
        Task SaveSelectedMonth(int journeyId, int selectedMonth);

        Task SaveSelectedWasteType(int journeyId, int selectedWasteTypeId);

        Task SaveSelectedWasteType(int journeyId, string selectedWasteType);

        Task<string> GetWasteType(int journeyId);
    }
}
