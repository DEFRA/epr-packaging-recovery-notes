namespace EPRN.PRNS.API.Models
{
    public class PackagingRecoveryNote : BaseEntity
    {
        public string ReferenceNumber { get; set; }     
        
        public double? Quantity { get; set; }

        public double? Adjustment { get; set; }

        public double? Tonnes { get; set; }

        public string? Note { get; set; }
    }
}