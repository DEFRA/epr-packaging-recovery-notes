using EPRN.Common.Dtos;

namespace Portal.RESTServices.Interfaces
{
    public interface IHttpWasteService
    {
        Task<IEnumerable<WasteTypeDto>> GetWasteMaterialTypes();

        Task SaveSelectedMonth(int journeyId, int selectedMonth);

        Task SaveSelectedWasteType(int journeyId, int selectedWasteTypeId);

        Task SaveWhatHaveYouDoneWaste(int journeyId, string whatHaveYouDoneWaste);

        Task<string> GetWasteType(int journeyId);
    }
}
