using EPRN.Common.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace EPRN.Common.Data.DataModels
{
    public class PackagingRecoveryNote : IdBaseEntity
    {
        public string Reference { get; set; }

        public string Note { get; set; }

        public int? WasteTypeId { get; set; }

        public Category Category { get; set; }

        public WasteType WasteType { get; set; }

        public int? WasteSubTypeId { get; set; }

        // The recipient of the PRN
        public string SentTo { get; set; }

        public double? Tonnes { get; set; }

        public int? SiteId { get; set; }

        public DateTime? CompletedDate { get; set; }

        public ICollection<PrnHistory> PrnHistory { get; set; }

        public DateTime CreatedDate { get; set; }

        
        public bool? DecemberWaste { get; set; }

        [MaxLength(36)]
        public string UserReferenceId { get; set; }
    }
}