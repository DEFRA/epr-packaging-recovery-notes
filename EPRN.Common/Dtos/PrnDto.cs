using EPRN.Common.Enums;

namespace EPRN.Common.Dtos
{
    public class PrnDto
    {
        public string PrnNumber { get; set; }
        public string Material { get; set; }
        public string SentTo { get; set; }
        public string DateCreated { get; set; }
        public double Tonnes { get; set; }
        public PrnStatus Status { get; set; }
        public string Link { get; set; }
    }
}
