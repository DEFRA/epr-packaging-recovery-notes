using EPRN.Common.Enums;

namespace EPRN.Portal.ViewModels.Waste
{
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
}
