using EPRN.Portal.ViewModels;
using EPRN.Portal.ViewModels.Waste;

namespace EPRN.Portal.Services.Interfaces
{
    public interface IHomeService
    {
        Task<HomePageViewModel> GetHomePage();
        double? GetBaledWithWireDeductionPercentage();
        Task<CheckAnswersViewModel> GetCheckAnswers(int journeyId);
    }
}
