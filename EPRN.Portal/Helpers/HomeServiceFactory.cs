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
        IEnumerable<IUserBasedService> _homeServices;

        public HomeServiceFactory(IEnumerable<IUserBasedService> homeServices, IUserRoleService userRoleService)
        {
            _homeServices = homeServices;
            _userRoleService = userRoleService;
        }

        public IUserBasedService CreateHomeService()
        {
            if (_userRoleService.HasRole(UserRole.Exporter) && _userRoleService.HasRole(UserRole.Reprocessor))
                return _homeServices.SingleOrDefault(x => x.GetType() == typeof(ExporterAndReprocessorHomeService));
            if (_userRoleService.HasRole(UserRole.Exporter))
                return _homeServices.SingleOrDefault(x => x.GetType() == typeof(UserBasedExporterService));
            if (_userRoleService.HasRole(UserRole.Reprocessor))
                return _homeServices.SingleOrDefault(x => x.GetType() == typeof(UserBasedReprocessorService));

            // default to exporter so that we do not throw an exception during start up
            return _homeServices.SingleOrDefault(x => x.GetType() == typeof(UserBasedExporterService));
        }

    }
}
