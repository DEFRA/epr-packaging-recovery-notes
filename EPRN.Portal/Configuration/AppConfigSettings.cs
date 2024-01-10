namespace EPRN.Portal.Configuration
{
    public class AppConfigSettings
    {
        public const string SectionName = "AppSettings";

        public double? DeductionAmount_Exporter { get; set; }
        public double? DeductionAmount_Reprocessor { get; set; }
        public double? DeductionAmount_ExporterAndReprocessor { get; set; }

    }
}
