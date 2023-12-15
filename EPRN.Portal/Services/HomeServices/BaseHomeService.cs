using EPRN.Portal.Services.Interfaces;

namespace EPRN.Portal.Services.HomeServices
{
    public abstract class BaseHomeService : IHomeService
    {
        public abstract Task<object> GetHomePage();

    }
}
