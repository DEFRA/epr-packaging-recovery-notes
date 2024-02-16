using EPRN.Common.Enums;

namespace EPRN.Common.Dtos
{
    public class DraftDetailsPrnDto
    {
        public int Id { get; set; }

        public string ReferenceNumber { get; set; }

        public PrnStatus Status { get; set; }
    }
}
