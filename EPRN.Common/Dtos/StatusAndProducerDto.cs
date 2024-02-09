using EPRN.Common.Enums;

namespace EPRN.Common.Dtos
{
    public class StatusAndProducerDto
    {
        public int Id { get; set; }

        public PrnStatus Status { get; set; }

        public string Producer { get; set; }

        public string Reference { get; set; }
    }
}