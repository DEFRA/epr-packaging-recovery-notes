using EPRN.Portal.ViewModels.Waste;

namespace EPRN.Portal.Services.Interfaces
{
    public interface IHomeService
    {
        Task<HomepageViewModel> GetHomePage();

    }
}
