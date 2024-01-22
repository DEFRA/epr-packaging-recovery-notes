using EPRN.Common.Enums;

namespace EPRN.Portal.ViewModels.Waste
{
    public class RecordWasteViewModel
    {
        public ExporterSectionViewModel ExporterSiteMaterials { get; set; }

        public ReproccesorSectionViewModel ReprocessorSiteMaterials { get; set; }
    }

    public class ExporterSectionViewModel : CategorySectionViewModel
    {
        public ExporterSectionViewModel()
        {
            Category = Category.Exporter;
        }
    }

    public class ReproccesorSectionViewModel : CategorySectionViewModel
    {
        public ReproccesorSectionViewModel()
        {
            Category = Category.Reprocessor;
        }
    }

    public abstract class CategorySectionViewModel
    {
        public Category Category { get; protected set; } // i.e. Reprocessor/Exporter

        public List<SiteSectionViewModel> Sites { get; set; }
    }

    public class SiteSectionViewModel
    {
        public string SiteName { get; set; }

        public Dictionary<int, string> SiteMaterials { get; set; }
    }
}