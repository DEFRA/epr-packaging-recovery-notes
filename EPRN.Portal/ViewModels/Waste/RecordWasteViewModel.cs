using EPRN.Common.Enums;

namespace EPRN.Portal.ViewModels.Waste
{
    public class RecordWasteViewModel
    {
        public int? Id { get; set; }

        public Category Category { get; set; }

        public ExporterSectionViewModel ExporterSiteMaterials { get; set; }

        public ReproccesorSectionViewModel ReprocessorSiteMaterials { get; set; }
    }
}