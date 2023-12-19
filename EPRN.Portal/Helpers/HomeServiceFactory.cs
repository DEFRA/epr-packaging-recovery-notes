using EPRN.Common.Enums;
using EPRN.Portal.Helpers.Interfaces;
using EPRN.Portal.Services.Interfaces;

namespace EPRN.Portal.Helpers
{
    public class HomeServiceFactory : IHomeServiceFactory
    {
        private readonly IHomeService exporterHomeService;
        private readonly IHomeService reprocessorHomeService;
        private readonly IHomeService exporterAndReprocessorHomeService;
        private readonly IWasteService wasteService;
        private readonly IUserRoleService userRoleService;

        public HomeServiceFactory(IHomeService exporterHomeService, IHomeService reprocessorHomeService, IHomeService exporterAndReprocessorHomeService, 
            IWasteService wasteService, IUserRoleService userRoleService)
        {
            this.exporterHomeService = exporterHomeService;
            this.reprocessorHomeService = reprocessorHomeService;
            this.exporterAndReprocessorHomeService = exporterAndReprocessorHomeService;
            this.wasteService = wasteService;
            this.userRoleService = userRoleService;
        }


        public IHomeService CreateHomeService(UserRole userRole)
        {
            switch (userRole)
            {
                case UserRole.Exporter: return exporterHomeService;

                case UserRole.Reprocessor: return reprocessorHomeService;

                case UserRole.ExporterAndReprocessor: return exporterAndReprocessorHomeService;

                default: return null;
            }
        }
    }
}
