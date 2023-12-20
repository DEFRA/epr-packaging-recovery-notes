using EPRN.Common.Enums;
using EPRN.Portal.Helpers.Interfaces;
using EPRN.Portal.Services;
using EPRN.Portal.Services.HomeServices;
using EPRN.Portal.Services.Interfaces;

namespace EPRN.Portal.Helpers
{
    public class HomeServiceFactory : IHomeServiceFactory
    {
        private readonly IUserRoleService _userRoleService;
        IEnumerable<IHomeService> _homeServices;

        public HomeServiceFactory(IEnumerable<IHomeService> homeServices, IUserRoleService userRoleService)
        {
            _homeServices = homeServices;
            _userRoleService = userRoleService;
        }

        public IHomeService CreateHomeService()
        {
            if (_userRoleService.HasRole(UserRole.Exporter) && _userRoleService.HasRole(UserRole.Reprocessor))
                return _homeServices.SingleOrDefault(x => x.GetType() == typeof(ExporterAndReprocessorHomeService));
            if (_userRoleService.HasRole(UserRole.Exporter))
                return _homeServices.SingleOrDefault(x => x.GetType() == typeof(ExporterHomeService));
            if (_userRoleService.HasRole(UserRole.Reprocessor))
                return _homeServices.SingleOrDefault(x => x.GetType() == typeof(ReprocessorHomeService));

            return null;
        }

    }
}
