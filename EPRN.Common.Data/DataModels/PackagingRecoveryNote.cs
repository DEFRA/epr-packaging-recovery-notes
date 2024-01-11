namespace EPRN.Common.Data.DataModels
{
    public class PackagingRecoveryNote : BaseEntity
    {
        public string Reference{ get; set; }     
      
        public string Note { get; set; }

        public int? WasteTypeId { get; set; }
        
        public WasteType WasteType { get; set; }

        public int? WasteSubTypeId { get; set; }

        public string SentTo { get; set; }

        public double? Tonnes { get; set; }

        public int? SiteId { get; set; }

        public DateTime? CompletedDate { get; set; }
    }
}