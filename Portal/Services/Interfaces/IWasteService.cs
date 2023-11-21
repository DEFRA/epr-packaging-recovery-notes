using Portal.ViewModels;

namespace Portal.Services.Interfaces
{
    public interface IWasteService
    {
        Task<WasteTypesViewModel> GetWasteTypesViewModel(int journeyId);
    }
}
