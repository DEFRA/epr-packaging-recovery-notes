using EPRN.Common.Dtos;
using EPRN.Common.Enums;

namespace Portal.RESTServices.Interfaces
{
    public interface IHttpWasteService
    {
        Task<IEnumerable<WasteTypeDto>> GetWasteMaterialTypes();
        Task<WasteRecordStatusDto> GetWasteRecordStatus(int journeyId);
        Task SaveSelectedMonth(int journeyId, int selectedMonth);

        Task SaveSelectedWasteType(int journeyId, int selectedWasteTypeId);

        Task SaveWhatHaveYouDoneWaste(int journeyId, DoneWaste whatHaveYouDoneWaste);

        Task<string> GetWasteType(int journeyId);
    }
}
