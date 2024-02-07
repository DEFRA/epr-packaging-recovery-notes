using EPRN.Common.Enums;

namespace EPRN.Common.Dtos
{
    public class WasteTonnageDto
    {
        public int Id { get; set; }

        public double? ExportTonnes { get; set; }

        public Category Category { get; set; }
    }
}