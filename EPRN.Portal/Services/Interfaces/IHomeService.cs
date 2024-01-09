using EPRN.Portal.ViewModels;

namespace EPRN.Portal.Services.Interfaces
{
    public interface IHomeService
    {
        Task<HomepageViewModel> GetHomePage();
        double? GetBaledWithWireDeductionPercentage();
    }
}
