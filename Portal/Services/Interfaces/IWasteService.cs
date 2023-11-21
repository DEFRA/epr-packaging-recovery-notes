using Portal.ViewModels.Waste;

namespace Portal.Services.Interfaces
{
    public interface IWasteService
    {
        Task<WasteTypeViewModel> GetWasteTypeViewModel(int journeyId);

        Task SaveSelectedWasteType(WasteTypeViewModel wasteTypeViewModel);
    }
}