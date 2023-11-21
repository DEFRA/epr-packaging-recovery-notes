namespace EPRN.Common.Models
{
    internal abstract class DtoBase : DtoIdBase
    {
        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public string LastModifiedBy { get; set; }
    }
}
