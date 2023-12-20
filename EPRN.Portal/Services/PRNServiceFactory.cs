using EPRN.Portal.Attributes;
using EPRN.Portal.Constants;
using EPRN.Portal.Services.Interfaces;

namespace EPRN.Portal.Services
{
    public class PRNServiceFactory : IPRNServiceFactory
    {
        private IPRNService _prnExporterService;
        private IPRNService _prnReprocessorService;

        public PRNServiceFactory(
            [Named(JourneyType.Reprocessor)] IPRNService prnExporterService,
            [Named(JourneyType.Exporter)] IPRNService prnReprocessorService)
        {
            _prnExporterService = prnExporterService ?? throw new ArgumentNullException(nameof(prnExporterService));
            _prnReprocessorService = prnReprocessorService ?? throw new ArgumentNullException(nameof(prnReprocessorService));
        }

        public IPRNService CreatePRNService(JourneyType journeyType) => journeyType switch
        {
            JourneyType.Exporter => _prnExporterService,
            JourneyType.Reprocessor => _prnReprocessorService,
            _ => throw new ArgumentException("No journey type, or invalid journey type specified")
        };
    }
}
