using EPRN.Portal.ViewModels;
using EPRN.Portal.ViewModels.Waste;

namespace EPRN.Portal.Services.Interfaces
{
    public interface IUserBasedService
    {
        Task<HomePageViewModel> GetHomePage();
        Task<CYAViewModel> GetCheckAnswers(int journeyId);
    }
}
