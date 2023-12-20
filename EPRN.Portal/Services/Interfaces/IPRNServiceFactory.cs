using EPRN.Portal.Constants;

namespace EPRN.Portal.Services.Interfaces
{
    public interface IPRNServiceFactory
    {
        IPRNService CreatePRNService(JourneyType journeyType);
    }
}
