#nullable enable
namespace EPRN.Waste.API.Configuration
{
    public class AppConfigSettings
    {
        public const string SectionName = "AppSettings";

        public double? DeductionAmount { get; set; }

        public List<int>? QuarterStartMonths { get; set; }

        public List<int>? ReturnDeadlineDays { get; set; }
        
        // ***** Test overrides
        // ***** Used to manipulate dates for test and QA purposes
        // ***** These will not contain values in production
        public DateTime? CurrentDateOverride { get; set; }          
        
        public bool? HasSubmittedReturnOverride { get; set; }
    }
}


