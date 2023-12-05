using Waste.API.Configuration.Interfaces;

namespace Waste.API.Configuration
{
    public class AppConfigSettings : IAppConfigSettings
    {
        public const string SectionName = "AppSettings";

        public double? DeductionAmount { get; set; }
    }
}
