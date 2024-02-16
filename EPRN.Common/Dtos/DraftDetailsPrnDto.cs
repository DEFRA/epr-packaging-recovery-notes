using EPRN.Common.Enums;

namespace EPRN.Common.Dtos
{
    public class DraftDetailsPrnDto
    {
        public string ReferenceNumber { get; set; }

        public PrnStatus Status { get; set; }
    }
}
