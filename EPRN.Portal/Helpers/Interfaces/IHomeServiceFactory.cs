using EPRN.Portal.Services.Interfaces;

namespace EPRN.Portal.Helpers.Interfaces
{
    public interface IHomeServiceFactory
    {
        IHomeService CreateHomeService();
    }
}
